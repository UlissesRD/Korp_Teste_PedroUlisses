using System.ComponentModel.DataAnnotations;

namespace Inventory.Api.DTOs;

public class CreateProductRequest
{
    [Required(ErrorMessage = "O codigo eh obrigatorio.")]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descricão eh obrigatoria.")]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "O saldo nao pode ser negativo.")]
    public int Balance { get; set; }
}