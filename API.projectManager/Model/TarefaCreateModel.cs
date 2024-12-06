using API.projectManager.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.projectManager.Model
{
    public class TarefaCreateModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da tarefa deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [StringLength(500, ErrorMessage = "A descrição da tarefa deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "Data de início inválida.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "Data de término inválida.")]
        [CustomValidation(typeof(Tarefa), "ValidarDataFim")]
        public DateTime DataFim { get; set; }

        [Required(ErrorMessage = "O status da tarefa é obrigatório.")]
        public string Status { get; set; }

        [Required]
        public int ProjetoId { get; set; }
    }
}
