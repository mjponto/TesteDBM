using Microsoft.EntityFrameworkCore;
using TesteDBM.Models;

namespace TesteDBM.Data.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _context.Produtos
            .AsNoTracking()  // Melhora performance para consultas readonly
            .ToListAsync();
    }

    public async Task<Produto?> ObterPorIdAsync(int id)
    {
        return await _context.Produtos.FindAsync(id);
    }

    public async Task<Produto> AdicionarAsync(Produto produto)
    {
        if (produto == null)
            throw new ArgumentNullException(nameof(produto));

        await _context.Produtos.AddAsync(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task<Produto?> AtualizarAsync(Produto produto)
    {
        if (produto == null)
            throw new ArgumentNullException(nameof(produto));

        var produtoExistente = await ObterPorIdAsync(produto.Id);
        if (produtoExistente == null) 
            return null;

        // Mantém a data de cadastro original
        produto.DataCadastro = produtoExistente.DataCadastro;
        
        _context.Entry(produtoExistente).CurrentValues.SetValues(produto);
        await _context.SaveChangesAsync();
        return produtoExistente;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var produto = await ObterPorIdAsync(id);
        if (produto == null) 
            return false;

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> NomeExisteAsync(string nome, int ignorarId = 0)
    {
        if (string.IsNullOrEmpty(nome))
            throw new ArgumentException("Nome não pode ser nulo ou vazio", nameof(nome));

        return await _context.Produtos
            .AsNoTracking()  // Melhora performance para consultas
            .AnyAsync(p => p.Nome == nome && p.Id != ignorarId);
    }
}