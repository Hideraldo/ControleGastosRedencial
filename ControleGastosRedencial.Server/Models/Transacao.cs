using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleGastosRedencial.Server.Models
{
    /// <summary>
    /// Entidade Transação: registra operações financeiras de despesa/receita.
    /// </summary>
    public class Transacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required]
        public TipoTransacao Tipo { get; set; }

        

        public int CategoriaId { get; set; }
        public int PessoaId { get; set; }

        /// <summary>Categoria relacionada à transação.</summary>
        public virtual Categoria? Categoria { get; set; }
        /// <summary>Pessoa relacionada à transação.</summary>
        public virtual Pessoa? Pessoa { get; set; }
    }
}
