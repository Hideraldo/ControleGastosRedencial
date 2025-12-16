namespace ControleGastosRedencial.Server.Models
{
    /// <summary>
    /// Entidade Categoria: classifica transações por finalidade e descrição.
    /// </summary>
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public TipoFinalidade Finalidade { get; set; }

        /// <summary>Transações associadas à categoria.</summary>
        public virtual ICollection<Transacao>? Transacaos { get; set; }
    }
}
