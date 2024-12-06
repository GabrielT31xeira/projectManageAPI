using API.projectManager.Entities;
using API.projectManager.Model;
using API.projectManager.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.projectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipesController : ControllerBase
    {
        private readonly ProjectManageDbContext _context;

        public EquipesController(ProjectManageDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adiciona o usuário autenticado à equipe especificada.
        /// </summary>
        /// <param name="id">ID da equipe.</param>
        /// <returns>Resposta indicando o resultado da operação.</returns>
        // POST: api/Equipes/EntrarNaEquipe
        [HttpPost("EntrarNaEquipe")]
        [Authorize]
        public async Task<IActionResult> EntrarNaEquipe(int id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var usuario_id = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == userEmail);
            if (usuario_id == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var userId = usuario_id.Id;

            var equipe = await _context.Equipes.Include(e => e.Usuarios).FirstOrDefaultAsync(e => e.Id == id);
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (equipe == null || usuario == null)
            {
                return NotFound("Equipe ou usuário não encontrado.");
            }

            if (!equipe.Usuarios.Contains(usuario))
            {
                equipe.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }

            return Ok("Usuário adicionado à equipe com sucesso.");
        }

        /// <summary>
        /// Remove o usuário autenticado da equipe especificada.
        /// </summary>
        /// <param name="id">ID da equipe.</param>
        /// <returns>Resposta indicando o resultado da operação.</returns>
        // POST: api/Equipes/SairDaEquipe
        [HttpPost("SairDaEquipe")]
        [Authorize]
        public async Task<IActionResult> SairDaEquipe(int id)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var usuario_id = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == userEmail);
            if (usuario_id == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var userId = usuario_id.Id;

            var equipe = await _context.Equipes.Include(e => e.Usuarios).FirstOrDefaultAsync(e => e.Id == id);
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (equipe == null || usuario == null)
            {
                return NotFound("Equipe ou usuário não encontrado.");
            }

            if (equipe.Usuarios.Contains(usuario))
            {
                equipe.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return Ok("Usuário removido da equipe com sucesso.");
        }

        /// <summary>
        /// Obtém a lista de todas as equipes, separando as equipes do usuário autenticado das outras equipes.
        /// </summary>
        /// <returns>Uma lista de equipes.</returns>
        // GET: api/Equipes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<EquipeViewModel>> Index()
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

            var userIdInt = usuario.Id;

            var minhasEquipes = await _context.Equipes
                                  .Include(e => e.Usuarios)
                                  .Where(e => e.Usuarios.Any(u => u.Id == userIdInt))
                                  .ToListAsync();

            var outrasEquipes = await _context.Equipes
                                       .Include(e => e.Usuarios)
                                       .Where(e => !e.Usuarios.Any(u => u.Id == userIdInt))
                                       .ToListAsync();

            var model = new EquipeViewModel
            {
                MinhasEquipes = minhasEquipes,
                OutrasEquipes = outrasEquipes
            };
            return Ok(model);
        }

        /// <summary>
        /// Obtém os detalhes de uma equipe específica pelo ID.
        /// </summary>
        /// <param name="id">ID da equipe.</param>
        /// <returns>Os detalhes da equipe.</returns>
        // GET: api/Equipes/Details/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Equipe>> Details(int id)
        {
            var equipe = await _context.Equipes.Include(e => e.Usuarios).FirstOrDefaultAsync(e => e.Id == id);
            if (equipe == null)
            {
                return NotFound("Equipe não encontrada.");
            }
            return Ok(equipe);
        }

        /// <summary>
        /// Cria uma nova equipe.
        /// </summary>
        /// <param name="model">Dados da nova equipe.</param>
        /// <returns>A equipe criada.</returns>
        // POST: api/Equipes/Create
        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult<Equipe>> Create(EquipeCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var equipe = new Equipe
                {
                    Nome = model.Nome,
                    Usuarios = new List<Usuario>()
                };

                _context.Equipes.Add(equipe);
                await _context.SaveChangesAsync();
                return Ok("Ação realizada com sucesso!");
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Atualiza os dados de uma equipe específica.
        /// </summary>
        /// <param name="id">ID da equipe.</param>
        /// <param name="equipe">Dados atualizados da equipe.</param>
        /// <returns>A equipe atualizada.</returns>
        // PUT: api/Equipes/Edit/5
        [HttpPut("Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, EquipeCreateModel equipe)
        {
            if (id != equipe.Id)
            {
                return BadRequest("ID da equipe não corresponde.");
            }

            if (ModelState.IsValid)
            {
                var equipeExistente = await _context.Equipes.FindAsync(id);
                if (equipeExistente == null)
                {
                    return NotFound("Projeto não encontrado.");
                }

                // Atualize as propriedades do projeto existente com os valores do modelo recebido
                equipeExistente.Nome = equipe.Nome;
                
                // Atualize outras propriedades conforme necessário

                _context.Entry(equipeExistente).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Equipes.Any(e => e.Id == id))
                    {
                        return NotFound("Equipe não encontrada.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok("Ação realizada com sucesso!");
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Exclui uma equipe específica.
        /// </summary>
        /// <param name="id">ID da equipe.</param>
        /// <returns>Resposta indicando o resultado da operação.</returns>
        // DELETE: api/Equipes/Delete/5
        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var equipe = await _context.Equipes.FindAsync(id);
            if (equipe == null)
            {
                return NotFound("Equipe não encontrada.");
            }

            _context.Equipes.Remove(equipe);
            await _context.SaveChangesAsync();
            return Ok("Ação realizada com sucesso!");
        }

        /// <summary>
        /// Remove um usuário específico de uma equipe.
        /// </summary>
        /// <param name="equipeId">ID da equipe.</param>
        /// <param name="usuarioId">ID do usuário.</param>
        /// <returns>Resposta indicando o resultado da operação.</returns>
        [HttpPost("RemoverUsuario")]
        [Authorize]
        public async Task<IActionResult> RemoverUsuario(int equipeId, int usuarioId)
        {
            var equipe = await _context.Equipes.Include(e => e.Usuarios).FirstOrDefaultAsync(e => e.Id == equipeId);
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (equipe == null || usuario == null)
            {
                return NotFound("Equipe ou usuário não encontrado.");
            }

            equipe.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuário removido da equipe com sucesso.");
        }
    }
}