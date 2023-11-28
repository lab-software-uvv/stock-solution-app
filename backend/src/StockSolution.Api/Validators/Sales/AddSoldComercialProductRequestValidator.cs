using FluentValidation;
using StockSolution.Api.Features.Sales;

namespace StockSolution.Api.Validators.Sales;

public class AddSoldComercialProductRequestValidator : Validator<AddSoldComercialProductRequest>
{
    public AddSoldComercialProductRequestValidator()
    {
        RuleFor(r => r.value).GreaterThan(0).WithMessage("Necessário informar um valor maior que zero");
        
        RuleFor(r => r.quantity).GreaterThan(0).WithMessage("Necessário informar uma quantidade maior que zero");
    }
}