using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace API.projectManager.Entities
{
    public class Projeto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do projeto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do projeto deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição do projeto é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição do projeto deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Projeto), "ValidarDataFim")]
        public DateTime DataFim { get; set; }

        // Relacionamento 1:N com Tarefa
        public virtual ICollection<Tarefa> Tarefas { get; set; }

        public int DonoId { get; set; }
        [ForeignKey("DonoId")]
        public virtual Usuario Dono { get; set; }

        // Relacionamento N:N com Usuario
        public virtual ICollection<Usuario> Usuarios { get; set; }

        // Método de validação para DataFim
        public static ValidationResult ValidarDataFim(DateTime dataFim, ValidationContext context)
        {
            var instance = context.ObjectInstance as Projeto;
            if (instance != null && dataFim < instance.DataInicio)
            {
                return new ValidationResult("A data de término não pode ser anterior à data de início.");
            }
            return ValidationResult.Success;
        }
    }
}
