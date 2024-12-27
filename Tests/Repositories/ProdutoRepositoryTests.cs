using Microsoft.EntityFrameworkCore;
using TesteDBM.Data;
using TesteDBM.Data.Repositories;
using TesteDBM.Models;
using Xunit;

namespace TesteDBM.Tests.Repositories;

public class ProdutoRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly ProdutoRepository _repository;

    public ProdutoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Filename=:memory:")
            .Options;

        _context = new AppDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        _repository = new ProdutoRepository(_context);
    }

    [Fact]
    public async Task AdicionarAsync_DeveSalvarERecuperarProduto()
    {
        // Arrange
        var produto = new Produto
        {
            Nome = "Produto Repositorio",
            Preco = 50m,
            DataCadastro = System.DateTime.Now
        };

        // Act
        var adicionado = await _repository.AdicionarAsync(produto);
        var recuperado = await _repository.ObterPorIdAsync(adicionado.Id);

        // Assert
        Assert.NotNull(recuperado);
        Assert.Equal("Produto Repositorio", recuperado!.Nome);
    }

    [Fact]
    public async Task RemoverAsync_DeveRemoverProduto()
    {
        // Arrange
        var produto = new Produto { Nome = "A Remover", Preco = 30m, DataCadastro = System.DateTime.Now };
        var adicionado = await _repository.AdicionarAsync(produto);

        // Act
        var removido = await _repository.RemoverAsync(adicionado.Id);
        var busca = await _repository.ObterPorIdAsync(adicionado.Id);

        // Assert
        Assert.True(removido);
        Assert.Null(busca);
    }
}