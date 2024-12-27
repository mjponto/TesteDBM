using TesteDBM.Models;

namespace TesteDBM.Data.Repositories;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorIdAsync(int id); 
    Task<Produto> AdicionarAsync(Produto produto);
    Task<Produto?> AtualizarAsync(Produto produto);
    Task<bool> RemoverAsync(int id);
    Task<bool> NomeExisteAsync(string nome, int ignorarId = 0);
}