// troque a porta pela que aparece no Swagger
const API_URL = "http://localhost:5217/api/Materiais";

const formulario = document.querySelector("#form");
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

let materiais = [];
let idEditando = null;

const ehLivro = m => Boolean(m.autor);

function mostrarMensagem(texto, tipoAlerta) {
  mensagem.textContent = texto;
  mensagem.classList.remove("d-none", "alert-success", "alert-danger");
  mensagem.classList.add("alert-" + tipoAlerta);
  setTimeout(() => mensagem.classList.add("d-none"), 4000);
}

function tratarErro(erro) {
  const semApi = erro.message === "Failed to fetch";
  mostrarMensagem(semApi ? "Não consegui conectar na API. Ela está rodando?" : erro.message, "danger");
}

// faz o fetch e devolve o texto da resposta; se response.ok for falso, lança um erro
function chamarApi(caminho, metodo, corpo) {
  return fetch(API_URL + caminho, {
    method: metodo,
    headers: { "Content-Type": "application/json" },
    body: corpo ? JSON.stringify(corpo) : undefined
  }).then(resposta => resposta.text().then(texto => {
    if (resposta.status === 404) { throw Error("Material não encontrado."); }
    if (!resposta.ok) { throw Error(texto && texto.length < 150 ? texto : `Erro ${resposta.status} na API.`); }
    return texto;
  }));
}

function carregar() {
  chamarApi("", "GET").then(texto => {
    materiais = JSON.parse(texto);
    mostrarLista();
  }).catch(tratarErro);
}

function mostrarLista() {
  lista.textContent = "";
  document.querySelector("#vazio").classList.toggle("d-none", materiais.length > 0);

  let livros = 0;
  materiais.forEach(function (m) {
    const livro = ehLivro(m);
    if (livro) { livros++; }
    const detalhe = livro
      ? `${m.autor}, ${m.numeroPaginas} páginas`
      : `Edição ${m.numeroEdicao}, mês ${m.mesPublicacao}`;

    const linha = document.createElement("tr");
    [m.id, m.titulo, m.anoPublicacao, livro ? "Livro" : "Revista", detalhe].forEach(function (valor) {
      const celula = document.createElement("td");
      celula.textContent = valor;
      linha.appendChild(celula);
    });

    // botões de ação: texto, classe do Bootstrap e função que cada um chama
    const acoes = document.createElement("td");
    [
      ["Ver", "btn-primary", () => ver("/" + m.id)],
      ["Editar", "btn-primary", () => editar(m.id)],
      ["Prazo", "btn-primary", () => ver("/" + m.id + "/descricao")],
      ["Emprestar", "btn-success", () => emprestar(m.id)],
      ["Excluir", "btn-danger", () => excluir(m.id)]
    ].forEach(function (acao) {
      const botao = document.createElement("button");
      botao.type = "button";
      botao.textContent = acao[0];
      botao.className = "btn btn-sm me-1 mb-1 " + acao[1];
      botao.addEventListener("click", acao[2]);
      acoes.appendChild(botao);
    });
    linha.appendChild(acoes);
    lista.appendChild(linha);
  });

  document.querySelector("#total").textContent = materiais.length;
  document.querySelector("#livros").textContent = livros;
  document.querySelector("#revistas").textContent = materiais.length - livros;
}

// mostra só os campos do tipo escolhido
function trocarTipo() {
  const livro = tipo.value === "livro";
  document.querySelectorAll(".livro").forEach(campo => campo.classList.toggle("d-none", !livro));
  document.querySelectorAll(".revista").forEach(campo => campo.classList.toggle("d-none", livro));
}

function limpar() {
  formulario.reset();
  formulario.querySelectorAll(".is-invalid").forEach(campo => campo.classList.remove("is-invalid"));
  idEditando = null;
  tipo.disabled = false;
  trocarTipo();
  document.querySelector("#form-titulo").textContent = "Cadastrar material";
  document.querySelector("#cancelar").classList.add("d-none");
}

// deixa o campo vermelho se estiver inválido e devolve true ou false
function validar(campo, invalido) {
  campo.classList.toggle("is-invalid", invalido);
  return !invalido;
}

// o mesmo formulário cadastra (POST) e edita (PUT)
formulario.addEventListener("submit", function (evento) {
  evento.preventDefault();

  let valido = true;
  const dados = { titulo: titulo.value.trim(), anoPublicacao: Number(ano.value) };

  if (!validar(titulo, dados.titulo === "")) { valido = false; }
  if (!validar(ano, dados.anoPublicacao < 1000 || dados.anoPublicacao > new Date().getFullYear())) { valido = false; }

  if (tipo.value === "livro") {
    dados.autor = autor.value.trim();
    dados.numeroPaginas = Number(paginas.value);
    if (!validar(autor, dados.autor === "")) { valido = false; }
    if (!validar(paginas, dados.numeroPaginas <= 0)) { valido = false; }
  } else {
    dados.numeroEdicao = Number(edicao.value);
    dados.mesPublicacao = Number(mes.value);
    if (!validar(edicao, dados.numeroEdicao <= 0)) { valido = false; }
    if (!validar(mes, dados.mesPublicacao < 1 || dados.mesPublicacao > 12)) { valido = false; }
  }

  if (!valido) {
    mostrarMensagem("Corrija os campos em vermelho.", "danger");
    return;
  }

  const caminho = "/" + tipo.value + (idEditando ? "/" + idEditando : "");
  chamarApi(caminho, idEditando ? "PUT" : "POST", dados).then(() => {
    mostrarMensagem(idEditando ? "Material atualizado!" : "Material cadastrado!", "success");
    limpar();
    carregar();
  }).catch(tratarErro);
});

function editar(id) {
  const m = materiais.find(item => item.id == id);
  idEditando = m.id;
  tipo.value = ehLivro(m) ? "livro" : "revista";
  tipo.disabled = true;
  titulo.value = m.titulo;
  ano.value = m.anoPublicacao;
  autor.value = m.autor ?? "";
  paginas.value = m.numeroPaginas ?? "";
  edicao.value = m.numeroEdicao ?? "";
  mes.value = m.mesPublicacao ?? "";
  trocarTipo();
  document.querySelector("#form-titulo").textContent = "Editar material";
  document.querySelector("#cancelar").classList.remove("d-none");
  formulario.scrollIntoView({ behavior: "smooth" });
}

function excluir(id) {
  if (!confirm("Tem certeza que quer excluir este material?")) { return; }

  chamarApi("/" + id, "DELETE").then(() => {
    mostrarMensagem("Material excluído!", "success");
  }).catch(tratarErro).then(carregar);
}

// GET /{id} e GET /{id}/descricao: mostra o que a API devolveu na caixinha de detalhes
function ver(caminho) {
  chamarApi(caminho, "GET").then(texto => {
    document.querySelector("#detalhes-texto").textContent = texto;
    detalhes.classList.remove("d-none");
  }).catch(tratarErro);
}

function emprestar(id) {
  chamarApi("/" + id + "/emprestar", "POST").then(texto => {
    mostrarMensagem(texto || "Empréstimo realizado!", "success");
  }).catch(tratarErro);
}

tipo.addEventListener("change", trocarTipo);
document.querySelector("#cancelar").addEventListener("click", limpar);
document.querySelector("#atualizar").addEventListener("click", carregar);
document.querySelector("#fechar-detalhes").addEventListener("click", () => detalhes.classList.add("d-none"));

carregar();