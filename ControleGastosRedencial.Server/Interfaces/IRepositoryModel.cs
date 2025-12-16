namespace ControleGastosRedencial.Server.Interfaces
{
    /// <summary>
    /// Contrato genérico para operações de repositório (CRUD) com entidades.
    /// </summary>
    public interface IRepositoryModel<T> where T : class
    {
        /// <summary>Lista todas as entidades.</summary>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>Obtém entidade pelo identificador.</summary>
        Task<T?> GetByIdAsync(int id);
        /// <summary>Adiciona uma nova entidade.</summary>
        Task<T> AddAsync(T entity);
        /// <summary>Atualiza uma entidade existente.</summary>
        Task UpdateAsync(T entity);
        /// <summary>Remove uma entidade pelo identificador.</summary>
        Task DeleteAsync(int id);
        /// <summary>Verifica se existe entidade com o identificador informado.</summary>
        Task<bool> ExistsAsync(int id);
    }
}
