using System.ComponentModel.DataAnnotations;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Models.Dtos
{
    /// <summary>
    /// DTO para criação/atualização de Transação via API.
    /// </summary>
    public class TransacaoDto
    {
        [Required]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required]
        public TipoTransacao Tipo { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public int PessoaId { get; set; }
    }
}
