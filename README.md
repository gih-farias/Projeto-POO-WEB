# Sistema de Gerenciamento de Biblioteca Universitária

## Integrantes

- **Giovanna Farias | 114.113**
- **Murillo Soares | 114.101**
- **Willian Kauê | 114.091**
- **Maria Clara | 114.105**

## Descrição do sistema
Sistema de **gerenciamento de biblioteca** que permite o cadastro, listagem, busca, atualização e exclusão de materiais do acervo, utilizando herança, abstração, polimorfismo, interface, Repository Pattern, injeção de dependência e Singleton.

## Tecnologias utilizadas
- **C#** - Linguagem de programação principal.
- **ASP.NET Core Web API**	- Framework utilizado para desenvolver a API.
- **Swagger (Swashbuckle)**	- Documentação e testes dos endpoints da API.

## Como executar
Siga os passos abaixo para clonar o repositório e executar o sistema localmente na sua máquina.

### Pré - Requisitos
Antes de começar, você vai precisar ter instalado:
- Git
- .NET 10 SDK

### 🚀 Passos para Execução

```bash
# 1. Clone este repositório para a sua máquina
$ git clone https://github.com/gih-farias/Projeto-POO-WEB.git

# 2. Acesse a pasta do projeto
$ cd Projeto-POO-WEB

# 3. Restaure as dependências do projeto
$ dotnet restore

# 4. Execute a aplicação
$ dotnet run

# 5. Acesse a documentação (Swagger) no navegador
# Abra a URL exibida no terminal seguida de /swagger
# Exemplo: http://localhost:PORTA/swagger
```

## Principais endpoints

| Método | URL | Função | Resposta esperada |
|---|---|---|---|
| GET | `/api/Materiais` | Lista todos | `200 OK` |
| GET | `/api/Materiais/{id}` | Busca por ID | `200 OK` ou `404` |
| POST | `/api/Materiais/livro` | Cadastra livro | `201 Created` |
| POST | `/api/Materiais/revista` | Cadastra revista | `201 Created` |
| PUT | `/api/Materiais/livro/{id}` | Atualiza livro | `200 OK` ou `404` |
| PUT | `/api/Materiais/revista/{id}` | Atualiza revista | `200 OK` ou `404` |
| DELETE | `/api/Materiais/{id}` | Exclui material | `204 No Content` ou `404` |
| GET | `/api/Materiais/{id}/descricao` | Mostra descrição e prazo | `200 OK` ou `404` |
| POST | `/api/Materiais/{id}/emprestar` | Tenta realizar empréstimo | `200 OK`, `400` ou `404` |

## Exemplos de testes
Todos os testes foram feitos pelo Swagger. Os dados ficam apenas em memória e começam com 1 livro (ID 1) e 1 revista (ID 2).

**Cadastrar livro** — `POST /api/Materiais/livro`
```json
{
  "titulo": "Percy Jackson e o Ladrão de Raios",
  "anoPublicacao": 2010,
  "autor": "Rick Riordan",
  "paginas": 400
}
```
Resultado: `201 Created` com o ID gerado pelo sistema.

**Cadastrar revista** — `POST /api/Materiais/revista`
```json
{
  "titulo": "Elle",
  "anoPublicacao": 1959,
  "edicao": 20,
  "mesPublicacao": 8
}
```
Resultado: `201 Created`.

**Validações** — resultado esperado: `400 Bad Request` com mensagem.
- Livro sem título ou com `paginas: 0`
- Revista com `edicao: 0` ou `mesPublicacao: 13`

**Polimorfismo** — `GET /api/Materiais/1/descricao` e `GET /api/Materiais/2/descricao`
- Livro: `prazoEmprestimo: 14`
- Revista: `prazoEmprestimo: 7`

## Evidências

Os prints dos testes no Swagger estão na pasta `evidencias/`.