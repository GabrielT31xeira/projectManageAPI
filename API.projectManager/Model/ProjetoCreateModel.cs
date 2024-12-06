using API.projectManager.Entities;
using System.ComponentModel.DataAnnotations;

namespace API.projectManager.Model
{
    public class ProjetoCreateModel
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
    }
}
