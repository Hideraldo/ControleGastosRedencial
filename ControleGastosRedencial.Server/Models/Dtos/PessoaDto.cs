using System.ComponentModel.DataAnnotations;
using ControleGastosRedencial.Server.Models;

namespace ControleGastosRedencial.Server.Models.Dtos
{
    /// <summary>
    /// DTO para criação/atualização de Pessoa via API.
    /// </summary>
    public class PessoaDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Range(0, 150)]
        public int Idade { get; set; }
    }
}
