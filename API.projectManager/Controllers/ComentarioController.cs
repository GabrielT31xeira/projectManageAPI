using API.projectManager.Entities;
using API.projectManager.Model;
using API.projectManager.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.projectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly ProjectManageDbContext _context;

        public ComentarioController(ProjectManageDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Adiciona um comentário a uma tarefa.
        /// </summary>
        /// <param name="tarefaId">ID da tarefa.</param>
        /// <param name="comentarioModel">Dados do comentário.</param>
        /// <returns>Resposta indicando o resultado da operação.</returns>
        [HttpPost("{tarefaId}")]
        public async Task<IActionResult> AddComentarioToTarefa(int tarefaId, [FromBody] ComentarioCreateModel comentarioModel)
        {
            var tarefa = await _context.Tarefas.Include(t => t.Comentarios).FirstOrDefaultAsync(t => t.Id == tarefaId);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            var comentario = new Comentario
            {
                Texto = comentarioModel.Texto,
                Data = comentarioModel.Data,
                TarefaId = tarefaId
            };

            tarefa.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return Ok("Comentário adicionado à tarefa com sucesso.");
        }

        /// <summary>
        /// Remove um comentário de uma tarefa.
        /// </summary>
        /// <param name="comentarioId">ID do comentário.</param>
        /// <returns>Resposta indicando o resultado da operação.</returns>
        [HttpDelete("{comentarioId}")]
        public async Task<IActionResult> RemoveComentarioFromTarefa(int comentarioId)
        {
            var comentario = await _context.Comentarios.FindAsync(comentarioId);

            if (comentario == null)
            {
                return NotFound("Tarefa ou comentário não encontrado.");
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();
            

            return Ok("Comentário removido da tarefa com sucesso.");
        }

        /// <summary>
        /// Obtém todos os comentários de uma tarefa específica.
        /// </summary>
        /// <param name="tarefaId">ID da tarefa.</param>
        /// <returns>Uma lista de comentários da tarefa.</returns>
        [HttpGet("{tarefaId}/comentarios")]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentariosDeTarefa(int tarefaId)
        {
            var tarefa = await _context.Tarefas
                .Include(t => t.Comentarios)
                .FirstOrDefaultAsync(t => t.Id == tarefaId);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            return Ok(tarefa.Comentarios);
        }
    }
}
