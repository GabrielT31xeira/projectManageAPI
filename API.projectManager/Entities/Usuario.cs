using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace API.projectManager.Entities
{
    public class Usuario
    {
        [Key]
        [Column("id_user")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [Column("email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Digite a senha.")]
        [Column("senha")]
        public string senha { get; set; }

        [Required(ErrorMessage = "Digite o nome.")]
        [Column("nome")]
        public string nome { get; set; }

        [Required(ErrorMessage = "Escolha um perfil.")]
        [Column("profile")]
        public string profile { get; set; }

        public string ConfirmationToken { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;

        public virtual ICollection<Projeto> Projetos { get; set; }

        [JsonIgnore]
        public virtual ICollection<Projeto> ProjetosComoDono { get; set; }

        public virtual ICollection<Equipe> Equipes { get; set; }
    }
}
