using FluentValidation;
using TesteDBM.Data.Repositories;
using TesteDBM.Models;

namespace TesteDBM.Validators;

public class ProdutoValidator : AbstractValidator<Produto>
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoValidator(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;

        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.")
            .MustAsync(async (produto, nome, cancellation) =>
            {
                return !await _produtoRepository.NomeExisteAsync(nome, produto.Id);
            })
            .WithMessage("Já existe um produto com este nome.");

        RuleFor(p => p.Preco)
            .NotNull().WithMessage("O preço é obrigatório.")
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");
    }
}