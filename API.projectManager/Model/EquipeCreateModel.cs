using System.ComponentModel.DataAnnotations;

namespace API.projectManager.Model
{
    public class EquipeCreateModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
    }
}
