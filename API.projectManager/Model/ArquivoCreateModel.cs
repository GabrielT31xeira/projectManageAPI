using System.ComponentModel.DataAnnotations;

namespace API.projectManager.Model
{
    public class ArquivoCreateModel
    {
        [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do arquivo deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O caminho do arquivo é obrigatório.")]
        public string Caminho { get; set; }
    }
}
