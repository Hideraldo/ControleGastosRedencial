using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Interfaces
{
    /// <summary>
    /// Contrato de repositório para Transação com consultas específicas e totais.
    /// </summary>
    public interface IRepositoryTransacao : IRepositoryModel<Transacao>
    {
        /// <summary>Lista transações por ID da pessoa.</summary>
        Task<IEnumerable<Transacao>> GetByPessoaIdAsync(int pessoaId);
        /// <summary>Lista transações por ID da categoria.</summary>
        Task<IEnumerable<Transacao>> GetByCategoriaIdAsync(int categoriaId);
        /// <summary>Lista transações por tipo (Despesa/Receita).</summary>
        Task<IEnumerable<Transacao>> GetByTipoAsync(TipoTransacao tipo);

        // Consultas de totais
        /// <summary>Obtém totais agregados por pessoa.</summary>
        Task<IEnumerable<TotalPorPessoa>> GetTotaisPorPessoaAsync();
        /// <summary>Obtém totais agregados por categoria.</summary>
        Task<IEnumerable<TotalPorCategoria>> GetTotaisPorCategoriaAsync();
        /// <summary>Obtém o total geral das transações.</summary>
        Task<TotalGeral> GetTotalGeralAsync();

        // Validação específica
        /// <summary>Valida se a transação é permitida para menor de idade conforme o tipo.</summary>
        Task<bool> ValidarTransacaoParaMenorAsync(int pessoaId, TipoTransacao tipo);
    }

    // DTOs para resultados das consultas
    /// <summary>Modelo de totalização por pessoa.</summary>
    public class TotalPorPessoa
    {
        public int PessoaId { get; set; }
        public string PessoaNome { get; set; } = string.Empty;
        public int PessoaIdade { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo => TotalReceitas - TotalDespesas;
    }

    /// <summary>Modelo de totalização por categoria.</summary>
    public class TotalPorCategoria
    {
        public int CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        public TipoFinalidade Finalidade { get; set; }
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo => TotalReceitas - TotalDespesas;
    }

    /// <summary>Modelo de totalização geral.</summary>
    public class TotalGeral
    {
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal SaldoLiquido => TotalReceitas - TotalDespesas;
    }
}
