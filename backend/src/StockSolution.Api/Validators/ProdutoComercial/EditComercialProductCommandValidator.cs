using FluentValidation;
using StockSolution.Api.Features.ComercialProducts;

namespace StockSolution.Api.Validators.Produto
{
    public class EditComercialProductCommandValidator : Validator<EditComercialProductCommand>
    {
        public EditComercialProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Campo Nome Deve Possuir de um Tamanho Máximo de 255 Caracteres");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Campo Código é de Preenchimento Obrigatório!")
                .MinimumLength(3)
                .WithMessage("Campo Código Deve Possuir um Tamanho Mínimo de 3 Caracteres")
                .MaximumLength(15)
                .WithMessage("Campo Nome Deve Possuir um Tamanho Máximo de 15 Caracteres")
                ;

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Campo Preço é de Preenchimento Obrigatório!")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Campo Preço Precisa Ser Maior ou Igual a Zero!")
                ;

            RuleFor(x => x.Description)
                .MaximumLength(255)
                .WithMessage("Campo Descrição Deve Possuir de um Tamanho Máximo de 255 Caracteres");

        }
    }
}
