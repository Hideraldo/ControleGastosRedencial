using Microsoft.EntityFrameworkCore;
using ControleGastosRedencial.Server.Interfaces;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Repositories
{
    /// <summary>
    /// Repositório de Transação com consultas específicas e validações de negócio.
    /// </summary>
    public class RepositoryTransacao : RepositoryBase<Transacao>, IRepositoryTransacao
    {
        /// <summary>
        /// Construtor que recebe o <see cref="ApplicationDbContext"/>.
        /// </summary>
        public RepositoryTransacao(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>Lista transações por ID da pessoa, incluindo navegação.</summary>
        public async Task<IEnumerable<Transacao>> GetByPessoaIdAsync(int pessoaId)
        {
            return await _dbSet
                .Include(t => t.Categoria)
                .Include(t => t.Pessoa)
                .Where(t => t.PessoaId == pessoaId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>Lista transações por ID da categoria, incluindo navegação.</summary>
        public async Task<IEnumerable<Transacao>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _dbSet
                .Include(t => t.Categoria)
                .Include(t => t.Pessoa)
                .Where(t => t.CategoriaId == categoriaId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>Lista transações por tipo (Despesa/Receita), incluindo navegação.</summary>
        public async Task<IEnumerable<Transacao>> GetByTipoAsync(TipoTransacao tipo)
        {
            return await _dbSet
                .Include(t => t.Categoria)
                .Include(t => t.Pessoa)
                .Where(t => t.Tipo == tipo)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>Calcula totais agregados por pessoa.</summary>
        public async Task<IEnumerable<TotalPorPessoa>> GetTotaisPorPessoaAsync()
        {
            var sqlAggregation = await _dbSet
                .Include(t => t.Pessoa)
                .GroupBy(t => new { t.PessoaId, t.Pessoa.Nome, t.Pessoa.Idade })
                .Select(g => new
                {
                    PessoaId = g.Key.PessoaId,
                    PessoaNome = g.Key.Nome,
                    PessoaIdade = g.Key.Idade,
                    TotalReceitas = g.Where(t => t.Tipo == TipoTransacao.Receita)
                                     .Sum(t => (double)t.Valor),
                    TotalDespesas = g.Where(t => t.Tipo == TipoTransacao.Despesa)
                                     .Sum(t => (double)t.Valor)
                })
                .AsNoTracking()
                .ToListAsync();

            return sqlAggregation
                .Select(x => new TotalPorPessoa
                {
                    PessoaId = x.PessoaId,
                    PessoaNome = x.PessoaNome,
                    PessoaIdade = x.PessoaIdade,
                    TotalReceitas = (decimal)x.TotalReceitas,
                    TotalDespesas = (decimal)x.TotalDespesas
                })
                .OrderBy(t => t.PessoaNome)
                .ToList();
        }

        /// <summary>Calcula totais agregados por categoria.</summary>
        public async Task<IEnumerable<TotalPorCategoria>> GetTotaisPorCategoriaAsync()
        {
            var sqlAggregation = await _dbSet
                .Include(t => t.Categoria)
                .GroupBy(t => new { t.CategoriaId, t.Categoria.Nome, t.Categoria.Finalidade })
                .Select(g => new
                {
                    CategoriaId = g.Key.CategoriaId,
                    CategoriaNome = g.Key.Nome,
                    Finalidade = g.Key.Finalidade,
                    TotalReceitas = g.Where(t => t.Tipo == TipoTransacao.Receita)
                                     .Sum(t => (double)t.Valor),
                    TotalDespesas = g.Where(t => t.Tipo == TipoTransacao.Despesa)
                                     .Sum(t => (double)t.Valor)
                })
                .AsNoTracking()
                .ToListAsync();

            return sqlAggregation
                .Select(x => new TotalPorCategoria
                {
                    CategoriaId = x.CategoriaId,
                    CategoriaNome = x.CategoriaNome,
                    Finalidade = x.Finalidade,
                    TotalReceitas = (decimal)x.TotalReceitas,
                    TotalDespesas = (decimal)x.TotalDespesas
                })
                .OrderBy(t => t.CategoriaNome)
                .ToList();
        }

        /// <summary>Calcula o total geral de receitas e despesas.</summary>
        public async Task<TotalGeral> GetTotalGeralAsync()
        {
            var totaisDouble = await _dbSet
                .GroupBy(t => 1)
                .Select(g => new
                {
                    TotalReceitas = g.Where(t => t.Tipo == TipoTransacao.Receita)
                                     .Sum(t => (double)t.Valor),
                    TotalDespesas = g.Where(t => t.Tipo == TipoTransacao.Despesa)
                                     .Sum(t => (double)t.Valor)
                })
                .FirstOrDefaultAsync();

            if (totaisDouble == null)
            {
                return new TotalGeral();
            }

            return new TotalGeral
            {
                TotalReceitas = (decimal)totaisDouble.TotalReceitas,
                TotalDespesas = (decimal)totaisDouble.TotalDespesas
            };
        }

        /// <summary>
        /// Valida se uma pessoa menor de idade pode registrar transação do tipo informado.
        /// </summary>
        public async Task<bool> ValidarTransacaoParaMenorAsync(int pessoaId, TipoTransacao tipo)
        {
            // Buscar pessoa para verificar idade
            var pessoa = await _context.Set<Pessoa>()
                .Where(p => p.Id == pessoaId)
                .Select(p => new { p.Idade })
                .FirstOrDefaultAsync();

            if (pessoa == null)
                return false;

            // Se for menor de idade (menor de 18), apenas despesas são permitidas
            if (pessoa.Idade < 18 && tipo == TipoTransacao.Receita)
            {
                return false; // Menor não pode ter receitas
            }

            return true; // Despesas são permitidas para todos, receitas apenas para maiores
        }

        /// <summary>
        /// Override de <see cref="RepositoryBase{T}.AddAsync(T)"/> com validações de negócio.
        /// </summary>
        public override async Task<Transacao> AddAsync(Transacao entity)
        {
            // Validar se menor pode ter este tipo de transação
            if (!await ValidarTransacaoParaMenorAsync(entity.PessoaId, entity.Tipo))
            {
                throw new InvalidOperationException(
                    "Menores de idade não podem cadastrar receitas. Apenas despesas são permitidas.");
            }

            // Validar se categoria pode ser usada para este tipo
            var categoriaRepo = new RepositoryCategoria(_context);
            if (!await categoriaRepo.CategoriaPodeSerUsadaParaTipoAsync(entity.CategoriaId, entity.Tipo))
            {
                throw new InvalidOperationException(
                    $"A categoria selecionada não pode ser usada para transações do tipo {entity.Tipo}.");
            }

            return await base.AddAsync(entity);
        }
    }
}
