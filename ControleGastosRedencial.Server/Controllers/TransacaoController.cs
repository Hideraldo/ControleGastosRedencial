using Microsoft.AspNetCore.Mvc;
using ControleGastosRedencial.Server.Interfaces;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controlador de Transações: CRUD e consultas agregadas de transações.
    /// </summary>
    public class TransacaoController : ControllerBase
    {
        private readonly IRepositoryTransacao _repositoryTransacao;
        private readonly IRepositoryPessoa _repositoryPessoa;
        private readonly IRepositoryCategoria _repositoryCategoria;

        /// <summary>
        /// Construtor que injeta repositórios de transação, pessoa e categoria.
        /// </summary>
        public TransacaoController(
            IRepositoryTransacao repositoryTransacao,
            IRepositoryPessoa repositoryPessoa,
            IRepositoryCategoria repositoryCategoria)
        {
            _repositoryTransacao = repositoryTransacao;
            _repositoryPessoa = repositoryPessoa;
            _repositoryCategoria = repositoryCategoria;
        }

        /// <summary>
        /// Lista todas as transações.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacaos()
        {
            var Transacaos = await _repositoryTransacao.GetAllAsync();
            return Ok(Transacaos);
        }

        /// <summary>
        /// Obtém uma transação pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Transacao>> GetTransacao(int id)
        {
            var transacao = await _repositoryTransacao.GetByIdAsync(id);

            if (transacao == null)
            {
                return NotFound($"Transação com ID {id} não encontrada.");
            }

            return Ok(transacao);
        }

        /// <summary>
        /// Lista transações por ID da pessoa.
        /// </summary>
        [HttpGet("pessoa/{pessoaId}")]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetPorPessoa(int pessoaId)
        {
            var Transacaos = await _repositoryTransacao.GetByPessoaIdAsync(pessoaId);
            return Ok(Transacaos);
        }

        /// <summary>
        /// Lista transações por ID da categoria.
        /// </summary>
        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetPorCategoria(int categoriaId)
        {
            var Transacaos = await _repositoryTransacao.GetByCategoriaIdAsync(categoriaId);
            return Ok(Transacaos);
        }

        /// <summary>
        /// Lista transações por tipo (Despesa ou Receita).
        /// </summary>
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<Transacao>>> GetPorTipo(TipoTransacao tipo)
        {
            var Transacaos = await _repositoryTransacao.GetByTipoAsync(tipo);
            return Ok(Transacaos);
        }

        /// <summary>
        /// Cria uma nova transação.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Transacao>> PostTransacao([FromBody] Models.Dtos.TransacaoDto transacaoDto)
        {
            if (transacaoDto == null)
            {
                return BadRequest("Dados da transação não podem ser nulos.");
            }

            // Validações básicas
            if (string.IsNullOrWhiteSpace(transacaoDto.Descricao))
            {
                return BadRequest("A descrição é obrigatória.");
            }

            if (transacaoDto.Descricao.Length > 200)
            {
                return BadRequest("A descrição não pode ter mais de 200 caracteres.");
            }

            if (transacaoDto.Valor <= 0)
            {
                return BadRequest("O valor deve ser maior que zero.");
            }

            // Verificar se pessoa existe
            var pessoa = await _repositoryPessoa.GetByIdAsync(transacaoDto.PessoaId);
            if (pessoa == null)
            {
                return BadRequest($"Pessoa com ID {transacaoDto.PessoaId} não encontrada.");
            }

            // Verificar se categoria existe
            var categoria = await _repositoryCategoria.GetByIdAsync(transacaoDto.CategoriaId);
            if (categoria == null)
            {
                return BadRequest($"Categoria com ID {transacaoDto.CategoriaId} não encontrada.");
            }

            try
            {
                var transacao = new Transacao
                {
                    Descricao = transacaoDto.Descricao,
                    Valor = transacaoDto.Valor,
                    Tipo = transacaoDto.Tipo,
                    CategoriaId = transacaoDto.CategoriaId,
                    PessoaId = transacaoDto.PessoaId
                };
                var transacaoCriada = await _repositoryTransacao.AddAsync(transacao);
                return CreatedAtAction(nameof(GetTransacao), new { id = transacaoCriada.Id }, transacaoCriada);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar transação: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza uma transação existente pelo ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransacao(int id, [FromBody] Models.Dtos.TransacaoDto transacaoDto)
        {
            var transacaoExistente = await _repositoryTransacao.GetByIdAsync(id);
            if (transacaoExistente == null)
            {
                return NotFound($"Transação com ID {id} não encontrada.");
            }

            // Verificar se pessoa existe
            var pessoa = await _repositoryPessoa.GetByIdAsync(transacaoDto.PessoaId);
            if (pessoa == null)
            {
                return BadRequest($"Pessoa com ID {transacaoDto.PessoaId} não encontrada.");
            }

            // Verificar se categoria existe
            var categoria = await _repositoryCategoria.GetByIdAsync(transacaoDto.CategoriaId);
            if (categoria == null)
            {
                return BadRequest($"Categoria com ID {transacaoDto.CategoriaId} não encontrada.");
            }

            try
            {
                transacaoExistente.Descricao = transacaoDto.Descricao;
                transacaoExistente.Valor = transacaoDto.Valor;
                transacaoExistente.Tipo = transacaoDto.Tipo;
                transacaoExistente.CategoriaId = transacaoDto.CategoriaId;
                transacaoExistente.PessoaId = transacaoDto.PessoaId;
                await _repositoryTransacao.UpdateAsync(transacaoExistente);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar transação: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove uma transação pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransacao(int id)
        {
            var transacao = await _repositoryTransacao.GetByIdAsync(id);
            if (transacao == null)
            {
                return NotFound($"Transação com ID {id} não encontrada.");
            }

            try
            {
                await _repositoryTransacao.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir transação: {ex.Message}");
            }
        }

        // CONSULTAS ESPECIAIS

        /// <summary>
        /// Retorna totais agregados por pessoa e o total geral.
        /// </summary>
        [HttpGet("totais/pessoas")]
        public async Task<ActionResult> GetTotaisPorPessoa()
        {
            var totais = await _repositoryTransacao.GetTotaisPorPessoaAsync();
            var geral = await _repositoryTransacao.GetTotalGeralAsync();

            return Ok(new
            {
                Pessoas = totais,
                TotalGeral = geral
            });
        }

        /// <summary>
        /// Retorna totais agregados por categoria e o total geral.
        /// </summary>
        [HttpGet("totais/categorias")]
        public async Task<ActionResult> GetTotaisPorCategoria()
        {
            var totais = await _repositoryTransacao.GetTotaisPorCategoriaAsync();
            var geral = await _repositoryTransacao.GetTotalGeralAsync();

            return Ok(new
            {
                Categorias = totais,
                TotalGeral = geral
            });
        }

        /// <summary>
        /// Retorna o total geral das transações.
        /// </summary>
        [HttpGet("total-geral")]
        public async Task<ActionResult<TotalGeral>> GetTotalGeral()
        {
            var totalGeral = await _repositoryTransacao.GetTotalGeralAsync();
            return Ok(totalGeral);
        }
    }
}
