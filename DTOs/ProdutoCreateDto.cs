namespace TesteDBM.DTOs;

public class ProdutoCreateDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
}