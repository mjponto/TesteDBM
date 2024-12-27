﻿namespace TesteDBM.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty; 
    public string? Descricao { get; set; }
    public decimal Preco { get; set; } 
    public DateTime DataCadastro { get; set; }  
}