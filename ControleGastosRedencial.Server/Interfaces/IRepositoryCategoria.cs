using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Interfaces
{
    /// <summary>
    /// Contrato de repositório para Categoria com consultas por nome e finalidade.
    /// </summary>
    public interface IRepositoryCategoria : IRepositoryModel<Categoria>
    {
        /// <summary>Busca categorias pelo nome (like).</summary>
        Task<IEnumerable<Categoria>> GetByNomeAsync(string nome);
        /// <summary>Busca categorias por finalidade, incluindo 'Ambas'.</summary>
        Task<IEnumerable<Categoria>> GetByFinalidadeAsync(TipoFinalidade finalidade);
        /// <summary>Valida se a categoria pode ser usada para o tipo de transação informado.</summary>
        Task<bool> CategoriaPodeSerUsadaParaTipoAsync(int categoriaId, TipoTransacao tipo);
    }
}
