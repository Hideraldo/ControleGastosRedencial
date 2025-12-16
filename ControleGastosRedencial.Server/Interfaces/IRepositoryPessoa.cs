using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Interfaces
{
    /// <summary>
    /// Contrato de repositório para Pessoa com consultas específicas.
    /// </summary>
    public interface IRepositoryPessoa : IRepositoryModel<Pessoa>
    {
        /// <summary>Busca pessoas pelo nome (like).</summary>
        Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome);
        /// <summary>Busca pessoas por faixa de idade.</summary>
        Task<IEnumerable<Pessoa>> GetByIdadeRangeAsync(int idadeMin, int idadeMax);
    }
}
