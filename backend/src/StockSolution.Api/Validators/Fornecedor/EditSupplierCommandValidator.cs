using FluentValidation;
using StockSolution.Api.Features.Suppliers;
using System.Text.RegularExpressions;

namespace StockSolution.Api.Validators.Fornecedor
{
    public class EditSupplierCommandValidator : Validator<EditSupplierCommand>
    {
        public EditSupplierCommandValidator()
        {
            RuleFor(x => x.Code)
                 .NotEmpty()
                 .WithMessage("Campo Código é de Preenchimento Obrigatório!")
                 .MinimumLength(3)
                 .WithMessage("Campo Código Deve Possuir um Tamanho Mínimo de 3 Caracteres")
                 .MaximumLength(15)
                 .WithMessage("Campo Código Deve Possuir um Tamanho Máximo de 15 Caracteres")
                 ;

            RuleFor(x => x.TradingName)
                .NotEmpty()
                .WithMessage("Campo Nome Fantasia é de Preenchimento Obrigatório!")
                .MinimumLength(3)
                .WithMessage("Campo Nome Fantasia Deve Possuir de um Tamanho Mínimo de 3 Caracteres");

            RuleFor(x => x.CNPJ)
                .NotEmpty()
                .WithMessage("Campo CNPJ é de Preenchimento Obrigatório!")
                .Length(14)
                .WithMessage("Campo CNPJ Deve Possuir 14 Caracteres")
                .Must(s => ValidarCNPJ(s!))
                .WithMessage("CNPJ Informado Não é Valido!"); ;

        }

        private static bool ValidarCNPJ(string cnpj)
        {

            // Calcular os dígitos verificadores
            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma = 0;

            for (int i = 0; i < 12; i++)
            {
                soma += int.Parse(cnpj[i].ToString()) * multiplicadores1[i];
            }

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            soma = 0;

            for (int i = 0; i < 13; i++)
            {
                soma += int.Parse(cnpj[i].ToString()) * multiplicadores2[i];
            }

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            // Verificar se os dígitos verificadores estão corretos
            return int.Parse(cnpj[12].ToString()) == digito1 && int.Parse(cnpj[13].ToString()) == digito2;
        }
    }
}
