using Microsoft.EntityFrameworkCore;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server
{
    /// <summary>
    /// DbContext principal da aplicação, define entidades e mapeamentos.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Construtor com opções do DbContext.
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>Tabela de pessoas.</summary>
        public DbSet<Pessoa> Pessoas { get; set; }
        /// <summary>Tabela de categorias.</summary>
        public DbSet<Categoria> Categorias { get; set; }
        /// <summary>Tabela de transações.</summary>
        public DbSet<Transacao> Transacaos { get; set; }

        /// <summary>
        /// Configura mapeamentos e restrições das entidades.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Pessoa
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(150);
                entity.Property(e => e.Idade)
                    .IsRequired();
            });

            // Configuração da entidade Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Descricao)
                    .HasMaxLength(500);
                entity.Property(e => e.Finalidade)
                    .IsRequired()
                    .HasConversion<int>(); // Converte enum para int no banco

                entity.HasIndex(e => e.Nome);
            });

            // Configuração da entidade Transacao
            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasConversion<int>();


                // Relacionamentos
                entity.HasOne(t => t.Categoria)
                    .WithMany(c => c.Transacaos)
                    .HasForeignKey(t => t.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Pessoa)
                    .WithMany(p => p.Transacaos)
                    .HasForeignKey(t => t.PessoaId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Índices para performance
                entity.HasIndex(t => t.Tipo);
                entity.HasIndex(t => t.PessoaId);
                entity.HasIndex(t => t.CategoriaId);
            });
        }
    }
}
