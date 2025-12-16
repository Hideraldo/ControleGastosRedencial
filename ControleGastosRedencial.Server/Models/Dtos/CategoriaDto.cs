using System.ComponentModel.DataAnnotations;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Models.Dtos
{
    /// <summary>
    /// DTO para criação/atualização de Categoria via API.
    /// </summary>
    public class CategoriaDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required]
        public TipoFinalidade Finalidade { get; set; }
    }
}
