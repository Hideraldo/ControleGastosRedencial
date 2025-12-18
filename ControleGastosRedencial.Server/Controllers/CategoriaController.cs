using Microsoft.AspNetCore.Mvc;
using ControleGastosRedencial.Server.Interfaces;
using ControleGastosRedencial.Server.Models;
using AutoMapper;
using ControleGastosRedencial.Server.Models.Dtos;

namespace ControleGastosRedencial.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controlador de Categorias: endpoints para CRUD e consultas por nome e finalidade.
    /// </summary>
    public class CategoriaController : ControllerBase
    {
        private readonly IRepositoryCategoria _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor que injeta o repositório de categorias.
        /// </summary>
        public CategoriaController(IRepositoryCategoria repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todas as categorias.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            var categorias = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CategoriaDto>>(categorias));
        }

        /// <summary>
        /// Obtém uma categoria pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> GetCategoria(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);

            if (categoria == null)
            {
                return NotFound($"Categoria com ID {id} não encontrada.");
            }

            return Ok(_mapper.Map<CategoriaDto>(categoria));
        }

        /// <summary>
        /// Busca categorias pelo nome (parâmetro de consulta).
        /// </summary>
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> BuscarPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return BadRequest("O parâmetro 'nome' é obrigatório.");
            }

            var categorias = await _repository.GetByNomeAsync(nome);
            return Ok(_mapper.Map<IEnumerable<CategoriaDto>>(categorias));
        }

        /// <summary>
        /// Lista categorias por finalidade (Despesa, Receita ou Ambas).
        /// </summary>
        [HttpGet("finalidade/{finalidade}")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetPorFinalidade(TipoFinalidade finalidade)
        {
            var categorias = await _repository.GetByFinalidadeAsync(finalidade);
            return Ok(_mapper.Map<IEnumerable<CategoriaDto>>(categorias));
        }

        /// <summary>
        /// Cria uma nova categoria.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> PostCategoria([FromBody] CategoriaDto categoriaDto)
        {
            if (categoriaDto == null)
            {
                return BadRequest("Dados da categoria não podem ser nulos.");
            }

            if (string.IsNullOrWhiteSpace(categoriaDto.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            }

            if (categoriaDto.Nome.Length > 100)
            {
                return BadRequest("O nome não pode ter mais de 100 caracteres.");
            }

            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                var categoriaCriada = await _repository.AddAsync(categoria);
                return CreatedAtAction(nameof(GetCategoria), new { id = categoriaCriada.Id }, _mapper.Map<CategoriaDto>(categoriaCriada));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar categoria: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza dados de uma categoria existente pelo ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, [FromBody] CategoriaDto categoriaDto)
        {
            if (string.IsNullOrWhiteSpace(categoriaDto.Nome))
            {
                return BadRequest("O nome é obrigatório.");
            }

            if (categoriaDto.Nome.Length > 100)
            {
                return BadRequest("O nome não pode ter mais de 100 caracteres.");
            }

            var categoriaExistente = await _repository.GetByIdAsync(id);
            if (categoriaExistente == null)
            {
                return NotFound($"Categoria com ID {id} não encontrada.");
            }

            try
            {
                _mapper.Map(categoriaDto, categoriaExistente);
                await _repository.UpdateAsync(categoriaExistente);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar categoria: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove uma categoria pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);
            if (categoria == null)
            {
                return NotFound($"Categoria com ID {id} não encontrada.");
            }

            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir categoria: {ex.Message}");
            }
        }
    }
}
