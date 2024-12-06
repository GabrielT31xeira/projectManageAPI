using API.projectManager.Entities;
using API.projectManager.Model;
using API.projectManager.Persistence;
using API.projectManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.projectManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ProjectManageDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ProjectManageDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Registra um novo usuário.
        /// </summary>
        /// <param name="model">Dados do usuário para registro.</param>
        /// <returns>Resultado da operação de registro.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            if (_context.Usuarios.Any(u => u.email == model.Email))
            {
                return BadRequest(new { message = "E-mail já está em uso." });
            }

            var usuario = new Usuario
            {
                email = model.Email,
                senha = passwordGenerator(model.Password),
                nome = model.Nome,
                profile = model.Profile,
                ConfirmationToken = GenerateToken(),
                IsEmailConfirmed = false
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            SendConfirmationEmail(usuario);

            return Ok(new { message = "Usuário registrado com sucesso. Verifique seu e-mail para confirmar a conta." });
        }

        /// <summary>
        /// Confirma o e-mail do usuário.
        /// </summary>
        /// <param name="token">Token de confirmação.</param>
        /// <returns>Resultado da operação de confirmação.</returns>
        [HttpGet("confirmemail")]
        [AllowAnonymous]
        public IActionResult ConfirmEmail(string token)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.ConfirmationToken == token);
            if (user != null)
            {
                user.IsEmailConfirmed = true;
                user.ConfirmationToken = "confirmed";
                _context.SaveChanges();
                return Ok(new { message = "E-mail confirmado com sucesso!" });
            }
            return BadRequest(new { message = "Token de confirmação inválido." });
        }

        /// <summary>
        /// Realiza o login do usuário.
        /// </summary>
        /// <param name="model">Dados do usuário para login.</param>
        /// <returns>Token JWT se o login for bem-sucedido.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _context.Usuarios.SingleOrDefault(u => u.email == model.Email);

            if (user == null || user.senha != passwordGenerator(model.Password))
            {
                return Unauthorized(new { message = "E-mail ou senha inválidos." });
            }

            var token = GenerateJwtToken(user.email, user.Id);
            return Ok(new { Token = token });
        }

        private string GenerateToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var token = new char[50];
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[50];
                cryptoProvider.GetBytes(data);
                for (int i = 0; i < token.Length; i++)
                {
                    token[i] = chars[data[i] % chars.Length];
                }
            }
            return new string(token);
        }

        private string GenerateJwtToken(string email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        new Claim(ClaimTypes.Email, email) // Adiciona o e-mail como um claim
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [NonAction]
        public void SendConfirmationEmail(Usuario user)
        {
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { token = user.ConfirmationToken }, protocol: HttpContext.Request.Scheme);
            string subject = "Confirmação de E-mail";
            string body = $"Clique no link para confirmar sua conta: {confirmationLink}";

            var emailService = new EmailService("smtp.mailtrap.io", 2525, "c90206253d8b79", "b58cd60b753e63");
            emailService.SendEmail(user.email, subject, body);
        }

        private string passwordGenerator(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}