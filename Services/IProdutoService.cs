using TesteDBM.Models;

namespace TesteDBM.Services;

public interface IProdutoService
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorIdAsync(int id);
    Task<Produto> AdicionarAsync(Produto produto);
    Task<Produto?> AtualizarAsync(int id, Produto produto);
    Task<bool> RemoverAsync(int id);
}