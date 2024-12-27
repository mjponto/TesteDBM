using Moq;
using TesteDBM.Data.Repositories;
using TesteDBM.Models;
using TesteDBM.Validators;
using Xunit;

namespace TesteDBM.Tests.Validators;

public class ProdutoValidatorTests
{
    private readonly ProdutoValidator _validator;
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock;

    public ProdutoValidatorTests()
    {
        _produtoRepositoryMock = new Mock<IProdutoRepository>();
        _validator = new ProdutoValidator(_produtoRepositoryMock.Object);
    }

    [Fact]
    public async Task Validator_DeveRetornarErro_SeNomeVazio()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "",
            Preco = 10m
        };

        // Act
        var resultado = await _validator.ValidateAsync(produto);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Nome");
    }

    [Fact]
    public async Task Validator_DeveRetornarErro_SePrecoForZeroOuNegativo()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "Produto Joao",
            Preco = 0
        };

        // Act
        var resultado = await _validator.ValidateAsync(produto);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Preco");
    }

    [Fact]
    public async Task Validator_DeveRetornarErro_SeNomeDuplicado()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "Duplicado",
            Preco = 10m
        };

        _produtoRepositoryMock
            .Setup(r => r.NomeExisteAsync("Duplicado", 0))
            .ReturnsAsync(true);

        // Act
        var resultado = await _validator.ValidateAsync(produto);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, e => e.PropertyName == "Nome");
    }
}