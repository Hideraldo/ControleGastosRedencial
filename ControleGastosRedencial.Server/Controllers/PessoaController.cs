using Microsoft.AspNetCore.Mvc;
using ControleGastosRedencial.Server.Interfaces;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controlador de Pessoas: expõe endpoints para CRUD e consultas de pessoas.
    /// </summary>
    public class PessoaController : ControllerBase
    {
        private readonly IRepositoryPessoa _repository;

        /// <summary>
        /// Construtor que injeta o repositório de pessoas.
        /// </summary>
        public PessoaController(IRepositoryPessoa repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lista todas as pessoas cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            var pessoas = await _repository.GetAllAsync();
            return Ok(pessoas);
        }

        /// <summary>
        /// Obtém uma pessoa pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var pessoa = await _repository.GetByIdAsync(id);

            if (pessoa == null)
            {
                return NotFound($"Pessoa com ID {id} não encontrada.");
            }

            return Ok(pessoa);
        }

        /// <summary>
        /// Busca pessoas pelo nome (parâmetro de consulta).
        /// </summary>
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> BuscarPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return BadRequest("O parâmetro 'nome' é obrigatório.");
            }

            var pessoas = await _repository.GetByNomeAsync(nome);
            return Ok(pessoas);
        }

        /// <summary>
        /// Lista pessoas por faixa de idade (mínima e máxima).
        /// </summary>
        [HttpGet("idade/{idadeMin}/{idadeMax}")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPorIdadeRange(int idadeMin, int idadeMax)
        {
            if (idadeMin < 0 || idadeMax < 0 || idadeMin > idadeMax)
            {
                return BadRequest("Idades inválidas. A idade mínima deve ser menor ou igual à máxima.");
            }

            var pessoas = await _repository.GetByIdadeRangeAsync(idadeMin, idadeMax);
            return Ok(pessoas);
        }

        /// <summary>
        /// Cria uma nova pessoa.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa([FromBody] Models.Dtos.PessoaDto pessoaDto)
        {
            if (pessoaDto == null)
            {
                return BadRequest("Dados da pessoa não podem ser nulos.");
            }

            if (string.IsNullOrWhiteSpace(pessoaDto.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            }

            if (pessoaDto.Nome.Length > 150)
            {
                return BadRequest("O nome não pode ter mais de 150 caracteres.");
            }

            if (pessoaDto.Idade < 0 || pessoaDto.Idade > 150)
            {
                return BadRequest("A idade deve estar entre 0 e 150 anos.");
            }

            try
            {
                var pessoa = new Pessoa
                {
                    Nome = pessoaDto.Nome,
                    Idade = pessoaDto.Idade
                };
                var pessoaCriada = await _repository.AddAsync(pessoa);
                return CreatedAtAction(nameof(GetPessoa), new { id = pessoaCriada.Id }, pessoaCriada);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar pessoa: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza dados de uma pessoa existente pelo ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, [FromBody] Models.Dtos.PessoaDto pessoaDto)
        {
            if (string.IsNullOrWhiteSpace(pessoaDto.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            }

            if (pessoaDto.Nome.Length > 150)
            {
                return BadRequest("O nome não pode ter mais de 150 caracteres.");
            }

            if (pessoaDto.Idade < 0 || pessoaDto.Idade > 150)
            {
                return BadRequest("A idade deve estar entre 0 e 150 anos.");
            }

            var pessoaExistente = await _repository.GetByIdAsync(id);
            if (pessoaExistente == null)
            {
                return NotFound($"Pessoa com ID {id} não encontrada.");
            }

            try
            {
                pessoaExistente.Nome = pessoaDto.Nome;
                pessoaExistente.Idade = pessoaDto.Idade;
                await _repository.UpdateAsync(pessoaExistente);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar pessoa: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove uma pessoa pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _repository.GetByIdAsync(id);
            if (pessoa == null)
            {
                return NotFound($"Pessoa com ID {id} não encontrada.");
            }

            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir pessoa: {ex.Message}");
            }
        }
    }
}
