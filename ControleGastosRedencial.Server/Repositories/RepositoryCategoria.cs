using Microsoft.EntityFrameworkCore;
using ControleGastosRedencial.Server.Interfaces;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Repositories
{
    /// <summary>
    /// Repositório de Categoria com consultas de nome, finalidade e validação de uso.
    /// </summary>
    public class RepositoryCategoria : RepositoryBase<Categoria>, IRepositoryCategoria
    {
        /// <summary>
        /// Construtor que recebe o <see cref="ApplicationDbContext"/>.
        /// </summary>
        public RepositoryCategoria(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>Busca categorias pelo nome (like).</summary>
        public async Task<IEnumerable<Categoria>> GetByNomeAsync(string nome)
        {
            return await _dbSet
                .Where(c => EF.Functions.Like(c.Nome, $"%{nome}%"))
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>Busca categorias por finalidade, incluindo 'Ambas'.</summary>
        public async Task<IEnumerable<Categoria>> GetByFinalidadeAsync(TipoFinalidade finalidade)
        {
            return await _dbSet
                .Where(c => c.Finalidade == finalidade || c.Finalidade == TipoFinalidade.Ambas)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Valida se a categoria pode ser usada para o tipo de transação informado.
        /// </summary>
        public async Task<bool> CategoriaPodeSerUsadaParaTipoAsync(int categoriaId, TipoTransacao tipo)
        {
            var categoria = await _dbSet
                .Where(c => c.Id == categoriaId)
                .Select(c => new { c.Finalidade })
                .FirstOrDefaultAsync();

            if (categoria == null)
                return false;

            return tipo switch
            {
                TipoTransacao.Despesa => categoria.Finalidade == TipoFinalidade.Despesa ||
                                         categoria.Finalidade == TipoFinalidade.Ambas,
                TipoTransacao.Receita => categoria.Finalidade == TipoFinalidade.Receita ||
                                         categoria.Finalidade == TipoFinalidade.Ambas,
                _ => false
            };
        }
    }
}
