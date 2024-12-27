using Moq;
using TesteDBM.Data.Repositories;
using TesteDBM.Models;
using TesteDBM.Services;
using TesteDBM.Validators;
using Xunit;

namespace TesteDBM.Tests.Services;

public class ProdutoServiceTests
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly ProdutoValidator _produtoValidator;
        private readonly ProdutoService _produtoService;

        public ProdutoServiceTests()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            
            _produtoValidator = new ProdutoValidator(_produtoRepositoryMock.Object);
            
            _produtoService = new ProdutoService(_produtoRepositoryMock.Object, _produtoValidator);
        }

        [Fact]
        public async Task AdicionarAsync_DeveRetornarProdutoQuandoValido()
        {
            // Arrange
            var produto = new Produto
            {
                Nome = "Produto Teste",
                Preco = 99.90m,
                DataCadastro = DateTime.Now
            };
            
            _produtoRepositoryMock
                .Setup(repo => repo.AdicionarAsync(It.IsAny<Produto>()))
                .ReturnsAsync((Produto p) => p);

            // Act
            var resultado = await _produtoService.AdicionarAsync(produto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Produto Teste", resultado.Nome);
            _produtoRepositoryMock.Verify(repo => repo.AdicionarAsync(produto), Times.Once);
        }

        [Fact]
        public async Task AdicionarAsync_DeveLancarExcecaoQuandoNomeDuplicado()
        {
            // Arrange
            var produto = new Produto
            {
                Nome = "Notebook Dell",
                Preco = 2000m,
                DataCadastro = DateTime.Now
            };
            
            _produtoRepositoryMock
                .Setup(repo => repo.NomeExisteAsync("Joao", 0))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _produtoService.AdicionarAsync(produto));
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarNullSeProdutoNaoExistir()
        {
            // Arrange
            var produto = new Produto { Id = 999, Nome = "JoaoV", Preco = 20m };
            
            _produtoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(produto.Id))
                .ReturnsAsync((Produto?)null);

            // Act
            var resultado = await _produtoService.AtualizarAsync(produto.Id, produto);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarQuandoValido()
        {
            // Arrange
            var produtoExistente = new Produto { Id = 10, Nome = "Antigo", Preco = 100m, DataCadastro = DateTime.Now.AddDays(-1) };
            var produtoNovo = new Produto { Id = 10, Nome = "Atualizado", Preco = 150m };
            
            _produtoRepositoryMock
                .Setup(repo => repo.ObterPorIdAsync(produtoExistente.Id))
                .ReturnsAsync(produtoExistente);
            
            _produtoRepositoryMock
                .Setup(repo => repo.AtualizarAsync(It.IsAny<Produto>()))
                .ReturnsAsync(produtoExistente);

            // Act
            var resultado = await _produtoService.AtualizarAsync(produtoNovo.Id, produtoNovo);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Atualizado", resultado!.Nome);
            Assert.Equal(150m, resultado.Preco);
        }

        [Fact]
        public async Task RemoverAsync_DeveRetornarFalseSeProdutoNaoExiste()
        {
            // Arrange
            int idInvalido = 999;
            _produtoRepositoryMock
                .Setup(repo => repo.RemoverAsync(idInvalido))
                .ReturnsAsync(false);

            // Act
            var resultado = await _produtoService.RemoverAsync(idInvalido);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task RemoverAsync_DeveRetornarTrueParaProdutoExistente()
        {
            // Arrange
            int idValido = 1;
            _produtoRepositoryMock
                .Setup(repo => repo.RemoverAsync(idValido))
                .ReturnsAsync(true);

            // Act
            var resultado = await _produtoService.RemoverAsync(idValido);

            // Assert
            Assert.True(resultado);
        }
    }