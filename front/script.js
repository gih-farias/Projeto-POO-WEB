const API_URL = "http://localhost:5217/api/Materiais";

const form = document.querySelector("#form");
const lista = document.querySelector("#lista");
const mensagem = document.querySelector("#mensagem");
const detalhes = document.querySelector("#detalhes");
const tipo = document.querySelector("#tipo");
const titulo = document.querySelector("#titulo");
const ano = document.querySelector("#ano");
const autor = document.querySelector("#autor");
const paginas = document.querySelector("#paginas");
const edicao = document.querySelector("#edicao");
const mes = document.querySelector("#mes");
const cancelar = document.querySelector("#cancelar");

let materiais = [];
let idEditando = null;

function mostrarMensagem(texto, tema) {
  mensagem.textContent = texto;
  mensagem.className = "alert theme-" + tema;
}

// GET /api/Materiais
function carregarMateriais() {
  fetch(API_URL)
    .then(resposta => {
      if (!resposta.ok) throw Error();
      return resposta.json();
    })
    .then(dados => {
      materiais = dados;
      mostrarMateriais();
    })
    .catch(() => mostrarMensagem("Erro ao carregar os materiais.", "danger"));
}

function mostrarMateriais() {
  lista.textContent = "";
  document.querySelector("#vazio").classList.toggle("d-none", materiais.length > 0);

  let livros = 0;
  materiais.forEach(function (m) {
    const ehLivro = m.tipo === "Livro";
    if (ehLivro) { livros++; }
    const info = ehLivro ? `${m.autor}, ${m.paginas} páginas` : `Edição ${m.edicao}, mês ${m.mesPublicacao}`;

    const linha = document.createElement("tr");
    [m.id, m.titulo, m.anoPublicacao, m.tipo, info].forEach(function (texto) {
      const celula = document.createElement("td");
      celula.textContent = texto;
      linha.appendChild(celula);
    });

    const acoes = document.createElement("td");
    [
      ["Ver", () => verDetalhes("/" + m.id)],
      ["Prazo", () => verDetalhes("/" + m.id + "/descricao")],
      ["Editar", () => prepararEdicao(m)],
      ["Emprestar", () => emprestarMaterial(m.id)],
      ["Excluir", () => excluirMaterial(m.id)]
    ].forEach(function (acao) {
      const botao = document.createElement("button");
      botao.textContent = acao[0];
      botao.className = "btn-outline btn-sm mb-1";
      botao.addEventListener("click", acao[1]);
      acoes.appendChild(botao);
    });
    linha.appendChild(acoes);
    lista.appendChild(linha);
  });

  document.querySelector("#total").textContent = materiais.length;
  document.querySelector("#livros").textContent = livros;
  document.querySelector("#revistas").textContent = materiais.length - livros;
}

// POST (cadastrar) e PUT (editar)
form.addEventListener("submit", function (evento) {
  evento.preventDefault();

  const dados = { titulo: titulo.value.trim(), anoPublicacao: Number(ano.value) };
  let valido = dados.titulo !== "" && dados.anoPublicacao > 0;

  if (tipo.value === "livro") {
    dados.autor = autor.value.trim();
    dados.paginas = Number(paginas.value);
    valido = valido && dados.autor !== "" && dados.paginas > 0;
  } else {
    dados.edicao = Number(edicao.value);
    dados.mesPublicacao = Number(mes.value);
    valido = valido && dados.edicao > 0 && dados.mesPublicacao >= 1 && dados.mesPublicacao <= 12;
  }

  if (!valido) {
    mostrarMensagem("Preencha todos os campos corretamente.", "danger");
    return;
  }

  const url = `${API_URL}/${tipo.value}` + (idEditando ? `/${idEditando}` : "");
  fetch(url, {
    method: idEditando ? "PUT" : "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(dados)
  })
    .then(resposta => {
      if (!resposta.ok) throw Error();
      mostrarMensagem(idEditando ? "Material atualizado!" : "Material cadastrado!", "success");
      limparFormulario();
      carregarMateriais();
    })
    .catch(() => mostrarMensagem("Não foi possível salvar o material.", "danger"));
});

function prepararEdicao(m) {
  idEditando = m.id;
  tipo.value = m.tipo === "Livro" ? "livro" : "revista";
  tipo.disabled = true;
  trocarCampos();
  titulo.value = m.titulo;
  ano.value = m.anoPublicacao;
  autor.value = m.autor ?? "";
  paginas.value = m.paginas ?? "";
  edicao.value = m.edicao ?? "";
  mes.value = m.mesPublicacao ?? "";
  document.querySelector("#form-titulo").textContent = "Editar material";
  cancelar.classList.remove("d-none");
}

// DELETE /api/Materiais/{id}
function excluirMaterial(id) {
  if (!confirm("Tem certeza que quer excluir este material?")) return;
  fetch(`${API_URL}/${id}`, { method: "DELETE" }).then(resposta => {
    mostrarMensagem(resposta.ok ? "Material excluído!" : "Material não encontrado.", resposta.ok ? "success" : "danger");
    carregarMateriais();
  });
}

// GET /api/Materiais/{id} e GET /api/Materiais/{id}/descricao
function verDetalhes(caminho) {
  fetch(API_URL + caminho)
    .then(resposta => {
      if (!resposta.ok) throw Error();
      return resposta.json();
    })
    .then(dados => {
      document.querySelector("#detalhes-texto").textContent = JSON.stringify(dados, null, 2);
      detalhes.classList.remove("d-none");
    })
    .catch(() => mostrarMensagem("Material não encontrado.", "danger"));
}

// POST /api/Materiais/{id}/emprestar
function emprestarMaterial(id) {
  fetch(`${API_URL}/${id}/emprestar`, { method: "POST" })
    .then(resposta => resposta.json().then(dados => {
      mostrarMensagem(resposta.ok ? dados.mensagem : dados.erro, resposta.ok ? "success" : "danger");
    }))
    .catch(() => mostrarMensagem("Material não encontrado.", "danger"));
}

function trocarCampos() {
  const ehLivro = tipo.value === "livro";
  document.querySelectorAll(".livro").forEach(campo => campo.classList.toggle("d-none", !ehLivro));
  document.querySelectorAll(".revista").forEach(campo => campo.classList.toggle("d-none", ehLivro));
}

function limparFormulario() {
  form.reset();
  idEditando = null;
  tipo.disabled = false;
  trocarCampos();
  document.querySelector("#form-titulo").textContent = "Cadastrar material";
  cancelar.classList.add("d-none");
}

tipo.addEventListener("change", trocarCampos);
cancelar.addEventListener("click", limparFormulario);
document.querySelector("#atualizar").addEventListener("click", carregarMateriais);
document.querySelector("#fechar-detalhes").addEventListener("click", () => detalhes.classList.add("d-none"));

carregarMateriais();