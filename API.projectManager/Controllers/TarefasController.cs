using API.projectManager.Entities;
using API.projectManager.Model;
using API.projectManager.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.projectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly ProjectManageDbContext _context;

        public TarefasController(ProjectManageDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém a lista de todas as tarefas de um projeto específico.
        /// </summary>
        /// <param name="projetoId">ID do projeto.</param>
        /// <returns>Uma lista de tarefas.</returns>
        // GET: api/Tarefas
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas(int projetoId)
        {
            return await _context.Tarefas
                .Include(t => t.Comentarios)
                .Include(t => t.Arquivos)
                .Where(t => t.ProjetoId == projetoId)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém uma tarefa específica pelo ID.
        /// </summary>
        /// <param name="id">ID da tarefa.</param>
        /// <returns>A tarefa correspondente ao ID.</returns>
        // GET: api/Tarefas/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Tarefa>> GetTarefa(int id)
        {
            var tarefa = await _context.Tarefas.Include(t => t.Comentarios).Include(t => t.Arquivos).FirstOrDefaultAsync(t => t.Id == id);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            return tarefa;
        }

        /// <summary>
        /// Cria uma nova tarefa para um projeto específico.
        /// </summary>
        /// <param name="projetoId">ID do projeto.</param>
        /// <param name="model">Dados da nova tarefa.</param>
        /// <returns>A tarefa criada.</returns>
        // POST: api/Tarefas
        [HttpPost("{projetoId}")]
        [Authorize]
        public async Task<ActionResult<Tarefa>> PostTarefa(int projetoId, TarefaCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var tarefa = new Tarefa
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    DataInicio = model.DataInicio,
                    DataFim = model.DataFim,
                    Status = model.Status,
                    ProjetoId = projetoId,
                    Comentarios = new List<Comentario>(),
                    Arquivos = new List<Arquivo>()
                };

                _context.Tarefas.Add(tarefa);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTarefa), new { projetoId = projetoId, id = tarefa.Id }, tarefa);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Exclui uma tarefa pelo ID.
        /// </summary>
        /// <param name="id">ID da tarefa a ser excluída.</param>
        /// <returns>Resposta HTTP indicando o resultado da operação.</returns>
        // DELETE: api/Tarefas/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TarefaExists(int id)
        {
            return _context.Tarefas.Any(e => e.Id == id);
        }
    }
}