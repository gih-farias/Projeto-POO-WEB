using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MinhaBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaisController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;

        public MateriaisController(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        // 1. GET - Lista todos
        [HttpGet]
        public IActionResult ObterTodos()
        {
            var materiais = _materialRepository.ObterTodos()
                .Select(material => new
                {
                    id = material.Id,
                    tipo = material.GetType().Name,
                    titulo = material.Titulo,
                    anoPublicacao = material.AnoPublicacao,
                    descricao = material.Descricao(),
                    prazoDias = material.CalcularPrazo(),
                    autor = material is Livro livro ? livro.Autor : null,
                    paginas = material is Livro livro2 ? livro2.Paginas : (int?)null,
                    edicao = material is Revista revista ? revista.Edicao : (int?)null,
                    mesPublicacao = material is Revista revista2 ? revista2.MesPublicacao : (int?)null
                });

            return Ok(materiais);
        }

        // 2. GET - Busca por ID (usando IActionResult para nao dar erro no Swagger com classe abstrata)
        [HttpGet("{id:int}")]
        public IActionResult ObterPorId(int id)
        {
            var material = _materialRepository.ObterPorId(id);

            if (material == null)
                return NotFound();

            return Ok(new
            {
                id = material.Id,
                tipo = material.GetType().Name,
                titulo = material.Titulo,
                anoPublicacao = material.AnoPublicacao,
                descricao = material.Descricao(),
                prazoDias = material.CalcularPrazo(),
                autor = material is Livro livro ? livro.Autor : null,
                paginas = material is Livro livro2 ? livro2.Paginas : (int?)null,
                edicao = material is Revista revista ? revista.Edicao : (int?)null,
                mesPublicacao = material is Revista revista2 ? revista2.MesPublicacao : (int?)null
            });
        }

        // 3. POST - Cadastra livro
        [HttpPost("livro")]
        public ActionResult<Livro> CadastraLivro([FromBody] Livro novoLivro)
        {
            if (novoLivro == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(novoLivro.Titulo))
                return BadRequest("O título é obrigatório.");

            if (novoLivro.AnoPublicacao < 0)
                return BadRequest("O ano de publicação não pode ser negativo.");

            if (string.IsNullOrWhiteSpace(novoLivro.Autor))
                return BadRequest("O autor é obrigatório.");

            if (novoLivro.Paginas <= 0)
                return BadRequest("O número de páginas deve ser maior que zero.");

            var cadastrado = _materialRepository.Adicionar(novoLivro);

            return CreatedAtAction(nameof(ObterPorId), new { id = cadastrado.Id }, cadastrado);
        }

        // 4. POST - Cadastra revista
        [HttpPost("revista")]
        public ActionResult<Revista> CadastraRevista([FromBody] Revista novaRevista)
        {
            if (novaRevista == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(novaRevista.Titulo))
                return BadRequest("O título é obrigatório.");

            if (novaRevista.AnoPublicacao < 0)
                return BadRequest("O ano de publicação não pode ser negativo.");

            if (novaRevista.Edicao <= 0)
                return BadRequest("O número da edição deve ser maior que zero.");

            if (novaRevista.MesPublicacao < 1 || novaRevista.MesPublicacao > 12)
                return BadRequest("O mês de publicação deve estar entre 1 e 12.");

            var cadastrada = _materialRepository.Adicionar(novaRevista);

            return CreatedAtAction(nameof(ObterPorId), new { id = cadastrada.Id }, cadastrada);
        }

        // 5. PUT - Atualiza livro
        [HttpPut("livro/{id:int}")]
        public IActionResult AtualizaLivro(int id, [FromBody] Livro livroAtualizado)
        {
            if (livroAtualizado == null)
                return BadRequest();

            var existente = _materialRepository.ObterPorId(id);
            if (existente == null)
                return NotFound();

            if (existente is not Livro)
                return BadRequest("O material informado não é um livro.");

            if (string.IsNullOrWhiteSpace(livroAtualizado.Titulo))
                return BadRequest("O título é obrigatório.");

            if (livroAtualizado.AnoPublicacao < 0)
                return BadRequest("O ano de publicação não pode ser negativo.");

            if (string.IsNullOrWhiteSpace(livroAtualizado.Autor))
                return BadRequest("O autor é obrigatório.");

            if (livroAtualizado.Paginas <= 0)
                return BadRequest("O número de páginas deve ser maior que zero.");

            _materialRepository.Atualizar(id, livroAtualizado);

            return Ok(livroAtualizado);
        }

        // 6. PUT - Atualiza revista
        [HttpPut("revista/{id:int}")]
        public IActionResult AtualizaRevista(int id, [FromBody] Revista revistaAtualizada)
        {
            if (revistaAtualizada == null)
                return BadRequest();

            var existente = _materialRepository.ObterPorId(id);
            if (existente == null)
                return NotFound();

            if (existente is not Revista)
                return BadRequest("O material informado não é uma revista.");

            if (string.IsNullOrWhiteSpace(revistaAtualizada.Titulo))
                return BadRequest("O título é obrigatório.");

            if (revistaAtualizada.AnoPublicacao < 0)
                return BadRequest("O ano de publicação não pode ser negativo.");

            if (revistaAtualizada.Edicao <= 0)
                return BadRequest("O número da edição deve ser maior que zero.");

            if (revistaAtualizada.MesPublicacao < 1 || revistaAtualizada.MesPublicacao > 12)
                return BadRequest("O mês de publicação deve estar entre 1 e 12.");

            _materialRepository.Atualizar(id, revistaAtualizada);

            return Ok(revistaAtualizada);
        }

        // 7. DELETE - Exclui material
        [HttpDelete("{id:int}")]
        public IActionResult ExcluiMaterial(int id)
        {
            bool removeu = _materialRepository.Remover(id);

            if (!removeu)
                return NotFound();

            return NoContent();
        }

        // 8. GET - Mostra descrição e prazo
        [HttpGet("{id:int}/descricao")]
        public IActionResult ObterDescricaoEPrazo(int id)
        {
            var material = _materialRepository.ObterPorId(id);

            if (material == null)
                return NotFound();

            var dados = new
            {
                id = material.Id,
                tipo = material.GetType().Name,
                descricao = material.Descricao(),
                prazoEmprestimo = material.CalcularPrazo()
            };

            return Ok(dados);
        }

        // 9. POST - Realiza empréstimo
        [HttpPost("{id:int}/emprestar")]
        public IActionResult RealizarEmprestimo(int id)
        {
            var material = _materialRepository.ObterPorId(id);

            if (material == null)
                return NotFound();

            if (material is not IEmprestimo materialComEmprestimo)
                return BadRequest(new { erro = "Este tipo de material não suporta empréstimos." });

            string resultado = materialComEmprestimo.Emprestar();
            return Ok(new { mensagem = resultado });
        }
    }
}
