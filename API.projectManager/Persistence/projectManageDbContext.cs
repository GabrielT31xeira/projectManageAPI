using API.projectManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.projectManager.Persistence
{
    public class ProjectManageDbContext : DbContext
    {
        public ProjectManageDbContext(DbContextOptions<ProjectManageDbContext> options) : base(options) { }

        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Arquivo> Arquivos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do relacionamento Dono
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.Dono)
                .WithMany(u => u.ProjetosComoDono)
                .HasForeignKey(p => p.DonoId)
                .OnDelete(DeleteBehavior.Restrict); // ou DeleteBehavior.Cascade, conforme necessário

            // Configuração do relacionamento N:N entre Projeto e Usuario
            modelBuilder.Entity<Projeto>()
                .HasMany(p => p.Usuarios)
                .WithMany(u => u.Projetos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjetoUsuarios",
                    j => j.HasOne<Usuario>().WithMany().HasForeignKey("UsuarioId"),
                    j => j.HasOne<Projeto>().WithMany().HasForeignKey("ProjetoId"));

            modelBuilder.Entity<Equipe>()
                .HasMany(e => e.Usuarios)
                .WithMany(u => u.Equipes)
                .UsingEntity<Dictionary<string, object>>(
                    "EquipeUsuarios",
                    j => j.HasOne<Usuario>().WithMany().HasForeignKey("UsuarioId"),
                    j => j.HasOne<Equipe>().WithMany().HasForeignKey("EquipeId"));

            modelBuilder.Entity<Tarefa>()
                .HasMany(t => t.Arquivos)
                .WithMany(a => a.Tarefas)
                .UsingEntity<Dictionary<string, object>>(
                    "TarefaArquivos",
                    j => j.HasOne<Arquivo>().WithMany().HasForeignKey("ArquivoId"),
                    j => j.HasOne<Tarefa>().WithMany().HasForeignKey("TarefaId"));

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}