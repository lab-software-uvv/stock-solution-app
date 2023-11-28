using FluentValidation;
using StockSolution.Api.Features.Sales;

namespace StockSolution.Api.Validators.Sales;

public class EditSaleRequestValidator : Validator<EditSaleRequest>
{
    public EditSaleRequestValidator()
    {
        RuleFor(r => r.sellingDate).NotEmpty().WithMessage("É obrigatório informar a data da venda.");

        RuleFor(r => r.totalValue)
            .GreaterThan(0).WithMessage("O valor da total da venda deve ser maior do que zero");

        RuleFor(r => r.paymentMethod)
            .IsInEnum()
            .NotEmpty().WithMessage("É obrigatório informar o método de pagamento");
    }
}