using FluentValidation;
using StockSolution.Api.Features.Categories;

namespace StockSolution.Api.Validators
{
    public class PostCategoriesRequestValidator: Validator<PostCategoriesRequest>
    {
        public PostCategoriesRequestValidator() 
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .WithMessage("Campo Nome é de Preenchimento Obrigatório!")
                .MinimumLength(3)
                .WithMessage("Campo Nome Deve Possuir um Tamanho Mínimo de 3 Caracteres")
                .MaximumLength(50)
                .WithMessage("Campo Nome Deve Possuir um Tamanho Máximo de 50 Caracteres")
                ;

            RuleFor(x => x.description)
                .MaximumLength(255)
                .WithMessage("Campo Descrição Deve Possuir de um Tamanho Máximo de 50 Caracteres");

        }
    }
}
