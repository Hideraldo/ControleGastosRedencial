using Microsoft.EntityFrameworkCore;
using ControleGastosRedencial.Server.Interfaces;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Repositories
{
    /// <summary>
    /// Repositório de Pessoa com consultas específicas e exclusão em cascata de transações.
    /// </summary>
    public class RepositoryPessoa : RepositoryBase<Pessoa>, IRepositoryPessoa
    {
        /// <summary>
        /// Construtor que recebe o <see cref="ApplicationDbContext"/>.
        /// </summary>
        public RepositoryPessoa(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>Busca pessoas pelo nome (like).</summary>
        public async Task<IEnumerable<Pessoa>> GetByNomeAsync(string nome)
        {
            return await _dbSet
                .Where(p => EF.Functions.Like(p.Nome, $"%{nome}%"))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>Busca pessoas por faixa de idade.</summary>
        public async Task<IEnumerable<Pessoa>> GetByIdadeRangeAsync(int idadeMin, int idadeMax)
        {
            return await _dbSet
                .Where(p => p.Idade >= idadeMin && p.Idade <= idadeMax)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Remove a pessoa e elimina transações relacionadas (se existirem).
        /// </summary>
        public override async Task DeleteAsync(int id)
        {
            var pessoa = await _dbSet.FindAsync(id);
            if (pessoa == null)
            {
                return;
            }

            var transacoes = await _context.Set<Transacao>()
                .Where(t => t.PessoaId == id)
                .ToListAsync();

            if (transacoes.Count > 0)
            {
                _context.Set<Transacao>().RemoveRange(transacoes);
            }

            _dbSet.Remove(pessoa);
            await _context.SaveChangesAsync();
        }
    }
}
