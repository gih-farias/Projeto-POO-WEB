# README da equipe

**Integrantes:**
- Murillo Macedo Soares
- Willian Kauê Da Cruz Campos
- Giovanna Farias Guanez
- Maria Clara Bogado Vieira

## O que foi desenvolvido?
Foi desenvolvido um sistema de gerenciamento de uma biblioteca universitária. A gente pensou em uma web que permite consultar, cadastrar, editar e excluir livros e revistas, além de visualizar informações sobre os materiais e seus prazos de empréstimo.

## Como executar a API?
1. Abra o projeto no VS Code.
2. Na pasta principal do projeto, rode `dotnet run`.
3. Aguarde a inicialização do servidor.
4. Abra o Swagger no endereço que aparece no terminal (algo como `https://localhost:PORTA/swagger`) para ver se a API está funcionando.

## Como abrir ou executar o front-end?
1. Abra a pasta `front` no VS Code.
2. Clique com o botão direito no arquivo `index.html`.
3. Escolha **Open with Live Server**.

A API precisa estar rodando antes de abrir o front.

## Qual URL da API foi utilizada?
http://localhost:5217/api/Materiais

(Se a porta for outra na sua máquina, é só trocar na constante `URL_API` no começo do `script.js`.)

## Quais endpoints foram consumidos?
- GET /api/Materiais: lista todos os materiais na tabela.
- GET /api/Materiais/{id}: mostra os dados de um material (botão Ver).
- GET /api/Materiais/{id}/descricao: mostra a descrição e o prazo de empréstimo (botão Prazo).
- POST /api/Materiais/livro e POST /api/Materiais/revista: cadastram um livro ou uma revista.
- PUT /api/Materiais/livro/{id} e PUT /api/Materiais/revista/{id}: atualizam um material (botão Editar).
- DELETE /api/Materiais/{id}: exclui um material, depois de pedir confirmação.
- POST /api/Materiais/{id}/emprestar: tenta emprestar. Livro dá certo, revista dá erro.

## Como o front-end foi organizado?
O front fica na pasta `front`, com três arquivos: `index.html` (a página com Bootstrap), `style.css` (o visual) e `script.js` (a lógica).

No `script.js` o código está dividido em funções: `carregarMateriais` busca a lista na API, `mostrarMateriais` desenha a tabela e os contadores, `verDetalhes` e `verPrazo` mostram as informações de um material, `prepararEdicao` joga os dados no formulário, `excluirMaterial` e `emprestarMaterial` chamam a API, e `mostrarStatus` mostra as mensagens de sucesso e erro. O formulário é um só: ele faz POST quando está cadastrando e PUT quando está editando.

## Quais componentes do Bootstrap foram utilizados?
Navbar, container, grid (row e col), cards, botões, alertas, tabela responsiva e campos de formulário (inputs e select).

## Quais validações foram implementadas?
- Título e ano de publicação precisam estar preenchidos.
- Livro: autor e número de páginas precisam estar preenchidos.
- Revista: número da edição e mês precisam estar preenchidos, e o mês tem que ser de 1 a 12.
- Antes de excluir, o sistema pede confirmação.

Quando algo está errado, aparece uma mensagem vermelha na tela e nada é enviado para a API.

## Quais dificuldades foram encontradas?
- O botão Buscar parecia não fazer nada quando clicavamos.
- As mensagens de sucesso e erro estavam sem cor.
- Em prazo de empréstimo, aparecia "undefined" no lugar do número de dias.
- O botão Emprestar parecia não funcionar.
- A lista só aparecia depois de clicar em Buscar.

## Como as dificuldades foram resolvidas?
- Os botões estavam invisíveis porque o Bootstrap 6 precisa de uma classe de cor junto com o `btn-outline`. Adicionamos `theme-secondary`, `theme-danger` e as outras.
- As mensagens ficaram sem cor porque usamos classes do Bootstrap 5 (`alert-success`). Trocamos para as do Bootstrap 6 (`theme-success` e `theme-danger`).
- O prazo dava "undefined" porque o nome do campo que a API devolve era diferente do que o código esperava. Fizemos o código procurar o campo que tem "prazo" no nome.
- O Emprestar funcionava, mas a mensagem aparecia lá no topo da página e a gente estava olhando a tabela. Fizemos a página rolar sozinha até a mensagem.
- Colocamos a chamada da lista para rodar quando a página abre, sem precisar clicar.

## O que cada integrante aprendeu?
- Murillo Macedo Soares: Aprendeu a por cor em bootstrap.
- Willian Kauê Da Cruz Campos: Aprendeu a adicionar um funcionalidade a um botão.
- Giovanna Guanez Farias: Aprendeu a incluir requisições em js.
- Maria Clara Bogado Vieira: Aprendeu a relacionar requisições ao html.


## Testes realizados
- Listar os materiais ao abrir a página e pelo botão Buscar.
- Cadastrar um livro e uma revista.
- Editar um material e conferir se a tabela atualizou.
- Excluir um material, com a confirmação.
- Ver os dados de um material (botão Ver) e o prazo (botão Prazo).
- Emprestar um livro (mensagem de sucesso) e uma revista (mensagem de erro).
- Testar a página em tela pequena.