using Microsoft.EntityFrameworkCore;
using ControleGastosRedencial.Server.Interfaces;

namespace ControleGastosRedencial.Server.Repositories
{
    /// <summary>
    /// Implementação base de repositório com operações CRUD usando Entity Framework Core.
    /// </summary>
    public abstract class RepositoryBase<T> : IRepositoryModel<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private bool saveChanges;

        /// <summary>
        /// Construtor padrão com <see cref="ApplicationDbContext"/>.
        /// </summary>
        protected RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        protected RepositoryBase(bool saveChanges)
        {
            this.saveChanges = saveChanges;
        }

        /// <summary>Lista todas as entidades sem rastreamento.</summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        /// <summary>Obtém entidade por ID.</summary>
        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>Adiciona entidade e salva alterações.</summary>
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>Atualiza entidade e salva alterações.</summary>
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>Remove entidade por ID (se existir) e salva alterações.</summary>
        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>Verifica existência de entidade por ID.</summary>
        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}
