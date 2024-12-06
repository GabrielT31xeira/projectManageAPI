using System.ComponentModel.DataAnnotations;

namespace API.projectManager.Entities
{
    public class Equipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        // Relação N:N com Usuario
        public virtual ICollection<Usuario> Usuarios { get; set; }

    }
}
