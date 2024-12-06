using API.projectManager.Entities;
using API.projectManager.Model;
using API.projectManager.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.projectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly ProjectManageDbContext _context;

        public ProjetoController(ProjectManageDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém a lista de todos os projetos.
        /// </summary>
        /// <returns>Uma lista de projetos.</returns>
        // GET: api/Projetos
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Projeto>>> GetProjetos()
        {
            return await _context.Projetos
                .Include(p => p.Tarefas)
                .Include(p => p.Dono) // Inclui o Dono
                .Include(p => p.Usuarios) // Inclui os Usuarios
                    .ToListAsync();
        }

        /// <summary>
        /// Obtém um projeto específico pelo ID.
        /// </summary>
        /// <param name="id">ID do projeto.</param>
        /// <returns>O projeto correspondente ao ID.</returns>
        // GET: api/Projetos/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Projeto>> GetProjeto(int id)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Tarefas)
                .Include(p => p.Dono) // Inclui o Dono
                .Include(p => p.Usuarios) // Inclui os Usuarios
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (projeto == null)
            {
                return NotFound("Projeto não encontrado.");
            }

            return projeto;
        }

        /// <summary>
        /// Cria um novo projeto.
        /// </summary>
        /// <param name="model">Dados do novo projeto.</param>
        /// <returns>O projeto criado.</returns>
        // POST: api/Projetos
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Projeto>> PostProjeto([FromBody] ProjetoCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (userEmail == null)
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == userEmail);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                var projeto = new Projeto
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    DataInicio = model.DataInicio,
                    DataFim = model.DataFim,
                    DonoId = usuario.Id,
                    Tarefas = new List<Tarefa>(),
                    Usuarios = new List<Usuario>()
                };

                _context.Projetos.Add(projeto);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProjeto), new { id = projeto.Id }, projeto);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Atualiza um projeto existente.
        /// </summary>
        /// <param name="id">ID do projeto a ser atualizado.</param>
        /// <param name="projeto">Dados atualizados do projeto. Lembre-se de mandar o id no body </param>
        /// <returns>Resposta HTTP indicando o resultado da operação.</returns>
        // PUT: api/Projetos/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProjeto(int id, ProjetoCreateModel projeto)
        {
            if (id != projeto.Id)
            {
                return BadRequest("ID do projeto não corresponde.");
            }

            if (ModelState.IsValid)
            {
                var projetoExistente = await _context.Projetos.FindAsync(id);
                if (projetoExistente == null)
                {
                    return NotFound("Projeto não encontrado.");
                }

                // Atualize as propriedades do projeto existente com os valores do modelo recebido
                projetoExistente.Nome = projeto.Nome;
                projetoExistente.Descricao = projeto.Descricao;
                projetoExistente.DataInicio = projeto.DataInicio;
                projetoExistente.DataFim = projeto.DataFim;
                // Atualize outras propriedades conforme necessário

                _context.Entry(projetoExistente).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetoExists(id))
                    {
                        return NotFound("Projeto não encontrado.");
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }


        /// <summary>
        /// Exclui um projeto pelo ID.
        /// </summary>
        /// <param name="id">ID do projeto a ser excluído.</param>
        /// <returns>Resposta HTTP indicando o resultado da operação.</returns>
        // DELETE: api/Projetos/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProjeto(int id)
        {
            var projeto = await _context.Projetos.FindAsync(id);
            if (projeto == null)
            {
                return NotFound("Projeto não encontrado.");
            }

            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjetoExists(int id)
        {
            return _context.Projetos.Any(e => e.Id == id);
        }
    }
}