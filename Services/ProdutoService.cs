using TesteDBM.Data.Repositories;
using TesteDBM.Models;
using TesteDBM.Validators;

namespace TesteDBM.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ProdutoValidator _produtoValidator;

    public ProdutoService(IProdutoRepository produtoRepository, ProdutoValidator produtoValidator)
    {
        _produtoRepository = produtoRepository;
        _produtoValidator = produtoValidator;
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _produtoRepository.ObterTodosAsync();
    }

    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _produtoRepository.ObterPorIdAsync(id);
    }

    public async Task<Produto> AdicionarAsync(Produto produto)
    {
        produto.DataCadastro = DateTime.Now;
        var validationResult = await _produtoValidator.ValidateAsync(produto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(string.Join(";", validationResult.Errors));
        }

        return await _produtoRepository.AdicionarAsync(produto);
    }

    public async Task<Produto?> AtualizarAsync(int id, Produto produto)
    {
        var produtoExistente = await _produtoRepository.ObterPorIdAsync(id);
        if (produtoExistente == null)
        {
            return null;
        }

        produto.Id = id; // Garante que o ID não seja alterado
        var validationResult = await _produtoValidator.ValidateAsync(produto);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(string.Join(";", validationResult.Errors));
        }

        return await _produtoRepository.AtualizarAsync(produto);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _produtoRepository.RemoverAsync(id);
    }
}