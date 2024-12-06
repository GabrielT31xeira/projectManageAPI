using API.projectManager.Entities;
using API.projectManager.Model;
using API.projectManager.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.projectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArquivoController : ControllerBase
    {
        private readonly ProjectManageDbContext _context;

        public ArquivoController(ProjectManageDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adiciona um arquivo a uma tarefa.
        /// </summary>
        /// <param name="tarefaId">ID da tarefa.</param>
        /// <param name="arquivoModel">Dados do arquivo.</param>
        /// <returns>Resposta indicando o resultado da operação.</returns>
        [HttpPost("{tarefaId}/tarefa")]
        public async Task<IActionResult> AddArquivoToTarefa(int tarefaId, [FromBody] ArquivoCreateModel arquivoModel)
        {
            var tarefa = await _context.Tarefas.Include(t => t.Arquivos).FirstOrDefaultAsync(t => t.Id == tarefaId);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            var arquivo = new Arquivo
            {
                Nome = arquivoModel.Nome,
                Caminho = arquivoModel.Caminho
            };

            tarefa.Arquivos.Add(arquivo);
            await _context.SaveChangesAsync();

            return Ok("Arquivo adicionado à tarefa com sucesso.");
        }

        /// <summary>
        /// Obtém todas as tarefas com seus arquivos associados.
        /// </summary>
        /// <returns>Uma lista de tarefas com arquivos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefasComArquivos()
        {
            var tarefas = await _context.Tarefas
                .Include(t => t.Arquivos)
                .ToListAsync();

            return Ok(tarefas);
        }

        /// <summary>
        /// Obtém os arquivos de uma tarefa específica.
        /// </summary>
        /// <param name="tarefaId">ID da tarefa.</param>
        /// <returns>Uma lista de arquivos da tarefa.</returns>
        [HttpGet("{tarefaId}/tarefa")]
        public async Task<ActionResult<IEnumerable<Arquivo>>> GetArquivosDeTarefa(int tarefaId)
        {
            var tarefa = await _context.Tarefas
                .Include(t => t.Arquivos)
                .FirstOrDefaultAsync(t => t.Id == tarefaId);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada.");
            }

            return Ok(tarefa.Arquivos);
        }

        /// <summary>
        /// Exclui um arquivo pelo ID.
        /// </summary>
        /// <param name="arquivoId">ID do arquivo a ser excluído.</param>
        /// <returns>Resposta HTTP indicando o resultado da operação.</returns>
        [HttpDelete("{arquivoId}")]
        public async Task<IActionResult> DeleteArquivo(int arquivoId)
        {
            var arquivo = await _context.Arquivos.FindAsync(arquivoId);
            if (arquivo == null)
            {
                return NotFound("Arquivo não encontrado.");
            }

            _context.Arquivos.Remove(arquivo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}