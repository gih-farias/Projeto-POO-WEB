
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;

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
                    descricao = material.Descricao(),
                    prazoDias = material.CalcularPrazo(),
                    anoPublicacao = material.AnoPublicacao,
                    titulo = material.Titulo,
                    // autor = material is Livro livro ? livro.Autor : null,
                    // paginas = material is Livro livro2 ? livro2.Paginas : (int?)null,
                    // edicao = material is Revista revista ? revista.Edicao : (int?)null,
                    // mesPublicacao = material is Revista revista2 ? revista2.MesPublicacao : (int?)null
                });

            return Ok(materiais);
        }

        // 2. GET - Busca por ID
        [HttpGet("{id:int}")]
        public ActionResult<MaterialBiblioteca> ObterPorId(int id)
        {
            var material = _materialRepository.ObterPorId(id);

            if (material == null)
                return NotFound();

            return Ok(material);
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

            if (novaRevista.AnoPublicacao <= 0)
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

            if (string.IsNullOrWhiteSpace(livroAtualizado.Titulo))
                return BadRequest("O título é obrigatório.");

            if (livroAtualizado.AnoPublicacao < 0)
                return BadRequest("O ano de publicação não pode ser negativo.");

            if (string.IsNullOrWhiteSpace(livroAtualizado.Autor))
                return BadRequest("O autor é obrigatório.");

            if (livroAtualizado.Paginas <= 0)
                return BadRequest("O número de páginas deve ser maior que zero.");

            bool atualizou = _materialRepository.Atualizar(id, livroAtualizado);

            if (!atualizou)
                return NotFound();

            return Ok(livroAtualizado);
        }

        // 6. PUT - Atualiza revista
        [HttpPut("revista/{id:int}")]
        public IActionResult AtualizaRevista(int id, [FromBody] Revista revistaAtualizada)
        {
            if (revistaAtualizada == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(revistaAtualizada.Titulo))
                return BadRequest("O título é obrigatório.");

            if (revistaAtualizada.AnoPublicacao < 0)
                return BadRequest("O ano de publicação não pode ser negativo.");

            if (revistaAtualizada.Edicao <= 0)
                return BadRequest("O número da edição deve ser maior que zero.");

            if (revistaAtualizada.MesPublicacao < 1 || revistaAtualizada.MesPublicacao > 12)
                return BadRequest("O mês de publicação deve estar entre 1 e 12.");

            bool atualizou = _materialRepository.Atualizar(id, revistaAtualizada);

            if (!atualizou)
                return NotFound();

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

            try
            {
                string resultado = materialComEmprestimo.Emprestar();

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}






// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Linq;
// using System.Collections.Generic;

// namespace MinhaBiblioteca.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")] // Define a rota base como /api/Materiais
//     public class MateriaisController : ControllerBase
//     {
//         private readonly IMaterialRepository _materialRepository;

//         public MateriaisController(IMaterialRepository materialRepository)
//         {
//             _materialRepository = materialRepository;
//         }

//         // 1. GET /api/Materiais -> Lista todos (200 OK)
//         [HttpGet]
//         public IActionResult ObterTodos()
//         {
//             var materiais = _materialRepository.ObterTodos()
//                 .Select(material => new
//                 {
//                     id = material.Id,
//                     tipo = material.GetType().Name,
//                     descricao = material.Descricao(),
//                     prazoDias = material.CalcularPrazo(),
//                     anoPublicacao = material.AnoPublicacao,
//                     titulo = material.Titulo
//                 });

//             return Ok(materiais);
//         }

//         // 2. GET /api/Materiais/{id} -> Busca por ID (200 OK ou 404)
//         [HttpGet("{id:int}")]
//         public ActionResult<MaterialBiblioteca> ObterPorId(int id)
//         {
//             var material = _materialRepository.ObterPorId(id);
//             if (material == null) return NotFound();
            
//             return Ok(material);
//         }

//         // 3. POST /api/Materiais/livro -> Cadastra livro (201 Created)
//         [HttpPost("livro")]
//         public ActionResult<Livro> CadastraLivro([FromBody] Livro novoLivro)
//         {
//             if (novoLivro == null){
//                 return BadRequest();
//             }

//             if(string.IsNullOrWhiteSpace(novoLivro.Titulo) || string.IsNullOrWhiteSpace(novoLivro.Autor))
//             {
//                 return BadRequest("Título e Autor são obrigatórios.");
//             }

//             var cadastrado = _materialRepository.Adicionar(novoLivro);
//             return Ok(cadastrado);
//         }

//         // 4. POST /api/Materiais/revista -> Cadastra revista (201 Created)
//         [HttpPost("revista")]
//         public ActionResult<Revista> CadastraRevista([FromBody] Revista novaRevista)
//         {
//             if (novaRevista == null){
//                 return BadRequest();
//             }

//             if(string.IsNullOrWhiteSpace(novaRevista.Titulo))
//             {
//                 return BadRequest("Título é obrigatório.");
//             }

//             var cadastrada = _materialRepository.Adicionar(novaRevista);
//             return Ok(cadastrada);
//         }

//         // 5. PUT /api/Materiais/livro/{id} -> Atualiza livro (200 OK ou 404)
//         [HttpPut("livro/{id:int}")]
//         public IActionResult AtualizaLivro(int id, [FromBody] Livro livroAtualizado)
//         {
//             if (livroAtualizado == null) return BadRequest();

//             bool atualizou = _materialRepository.Atualizar(id, livroAtualizado);
//             if (!atualizou) return NotFound();

//             return Ok(livroAtualizado); // Retorna 200 OK conforme pedido na tabela
//         }

//         // 6. PUT /api/Materiais/revista/{id} -> Atualiza revista (200 OK ou 404)
//         [HttpPut("revista/{id:int}")]
//         public IActionResult AtualizaRevista(int id, [FromBody] Revista revistaAtualizada)
//         {
//             if (revistaAtualizada == null) return BadRequest();

//             bool atualizou = _materialRepository.Atualizar(id, revistaAtualizada);
//             if (!atualizou) return NotFound();

//             return Ok(revistaAtualizada); // Retorna 200 OK conforme pedido na tabela
//         }

//         // 7. DELETE /api/Materiais/{id} -> Exclui material (204 No Content ou 404)
//         [HttpDelete("{id:int}")]
//         public IActionResult ExcluiMaterial(int id)
//         {
//             bool removeu = _materialRepository.Remover(id);
//             if (!removeu) return NotFound();

//             return NoContent(); // Retorna 204 No Content estritamente
//         }

//         // 8. GET /api/Materiais/{id}/descricao -> Mostra descrição e prazo (200 OK)
//         [HttpGet("{id:int}/descricao")]
//         public IActionResult ObterDescricaoEPrazo(int id)
//         {
//             var material = _materialRepository.ObterPorId(id);
//             if (material == null) return NotFound();

//             // Executa o polimorfismo dinamicamente com base no tipo real do objeto
//             var dados = new
//             {
//                 Descricao = material.Descricao(),
//                 PrazoDias = material.CalcularPrazo()
//             };

//             return Ok(dados);
//         }

//         // 9. POST /api/Materiais/{id}/emprestar -> Tenta realizar empréstimo (200 OK, 400 ou 404)
//         [HttpPost("{id:int}/emprestar")]
//         public IActionResult RealizarEmprestimo(int id)
//         {
//             var material = _materialRepository.ObterPorId(id);
//             if (material == null) return NotFound(); // 404 se não existir

//             // Verifica se o objeto estende ou implementa o comportamento de empréstimo
//             if (material is IEmprestimo materialComEmprestimo)
//             {
//                 try
//                 {
//                     string resultado = materialComEmprestimo.Emprestar();
//                     return Ok(new { mensagem = resultado }); // 200 OK em caso de sucesso
//                 }
//                 catch (Exception ex)
//                 {
//                     // Caso haja uma lógica de negócio interna que barre o empréstimo
//                     return BadRequest(new { erro = ex.Message }); // 400 Bad Request
//                 }
//             }

//             // Se o material for encontrado mas NÃO for um tipo emprestável (ex: uma Revista sem a interface)
//             return BadRequest(new { erro = "Este tipo de material não suporta empréstimos." }); // 400 Bad Request
//         }
//     }
// }