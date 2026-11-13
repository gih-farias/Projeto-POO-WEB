# Sistema de Gerenciamento de Biblioteca Universitária

## Integrantes

*Giovanna Farias | 114.113*
*Murillo Soares | 114.101*
*Willian Kauê | 114.091*
*Maria Clara | 114.105*

## Descrição do sistema
Sistema de *gerenciamento de biblioteca* que permite o cadastro, listagem, busca, atualização e exclusão de materiais do acervo, utilizando herança, abstração, polimorfismo, interface, Repository Pattern, injeção de dependência e Singleton.

## Tecnologias utilizadas
*C#* - Linguagem de programação principal.
*ASP.NET Core Web API*	- Framework utilizado para desenvolver a API.
*Swagger (Swashbuckle)*	- Documentação e testes dos endpoints da API.

## Como executar
Siga os passos abaixo para clonar o repositório e executar o sistema localmente na sua máquina.

### Pré - Requisitos
Antes de começar, você vai precisar ter instalado:
*Git
*.NET 10 SDK

### 🚀 Passos para Execução

```bash
# 1. Clone este repositório para a sua máquina
$ git clone https://github.com

# 2. Acesse a pasta do projeto
$ cd MinhaBiblioteca

# 3. Restaure as dependências do projeto
$ dotnet restore

# 4. Execute a aplicação
$ dotnet run

# 5. Acesse a documentação (Swagger) no navegador
# Abra a URL exibida no terminal seguida de /swagger
# Exemplo: http://localhost:5000/swagger
```

## Principais endpoints

| Método | URL | Função | Resposta esperada |
|---|---|---|---|
| GET | `/api/Materiais` | Lista todos | `200 OK` |
| POST | `/api/Materiais/livro` | Cadastra livro | `201 Created` |
| POST | `/api/Materiais/revista` | Cadastra revista | `201 Created` |
| PUT | `/api/Materiais/livro/{id}` | Atualiza livro | `200 OK` ou `404` |
| PUT | `/api/Materiais/revista/{id}` | Atualiza revista | `200 OK` ou `404` |
| DELETE | `/api/Materiais/{id}` | Exclui material | `204 No Content` ou `404` |

## Exemplos de testes