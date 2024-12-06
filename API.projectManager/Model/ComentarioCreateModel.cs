using System.ComponentModel.DataAnnotations;

namespace API.projectManager.Model
{
    public class ComentarioCreateModel
    {
        [Required(ErrorMessage = "O texto do comentário é obrigatório.")]
        [StringLength(500, ErrorMessage = "O texto do comentário deve ter no máximo 500 caracteres.")]
        public string Texto { get; set; }

        [Required(ErrorMessage = "A data do comentário é obrigatória.")]
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }
    }
}
