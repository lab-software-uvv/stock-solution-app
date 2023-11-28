using FluentValidation;
using StockSolution.Api.Features.Sales;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Validators.Sales;

public class CreateSaleRequestValidator : Validator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {
        RuleFor(r => r.userId).NotEmpty().WithMessage("O id do usuario é de Preenchimento Obrigatório!");

        RuleFor(r => r.sellingDate).NotEmpty().WithMessage("É obrigatório informar a data da venda.");

        RuleFor(r => r.totalValue)
            .GreaterThan(0).WithMessage("O valor da total da venda deve ser maior do que zero");

        RuleFor(r => r.paymentMethod)
            .IsInEnum()
            .NotEmpty().WithMessage("É obrigatório informar o método de pagamento");
    }
}