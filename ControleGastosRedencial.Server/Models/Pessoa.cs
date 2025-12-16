using System.ComponentModel.DataAnnotations;

namespace ControleGastosRedencial.Server.Models
{
    /// <summary>
    /// Entidade Pessoa: representa um indivíduo com nome e idade.
    /// </summary>
    public class Pessoa
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Range(0, 150)]
        public int Idade { get; set; }

        /// <summary>Lista de transações relacionadas à pessoa.</summary>
        public virtual ICollection<Transacao>? Transacaos { get; set; }

        /// <summary>Indica se a pessoa é menor de idade (menos de 18 anos).</summary>
        public bool IsMenorIdade => Idade < 18;
    }
}
