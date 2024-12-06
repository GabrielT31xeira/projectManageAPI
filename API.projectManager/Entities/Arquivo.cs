using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace API.projectManager.Entities
{
    public class Arquivo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do arquivo deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O caminho do arquivo é obrigatório.")]
        public string Caminho { get; set; }

        // Relação N:N com Tarefa
        public virtual ICollection<Tarefa> Tarefas { get; set; }
    }
}
