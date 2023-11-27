﻿using FluentValidation;
using StockSolution.Api.Features.Categories;
using StockSolution.Api.Features.Products;

namespace StockSolution.Api.Validators.Produto
{
    public class EditProductCommandValidator : Validator<EditProductCommand>
    {
        public EditProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Campo Descrição Deve Possuir de um Tamanho Máximo de 50 Caracteres");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Campo Código é de Preenchimento Obrigatório!")
                .MinimumLength(3)
                .WithMessage("Campo Código Deve Possuir um Tamanho Mínimo de 3 Caracteres")
                .MaximumLength(15)
                .WithMessage("Campo Nome Deve Possuir um Tamanho Máximo de 15 Caracteres")
                ;

            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("Campo Quantidade é de Preenchimento Obrigatório!")
                .GreaterThan(0)
                .WithMessage("Campo Quantidade Precisa Ser Maior que Zero!")
                ;

            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .WithMessage("Campo Fornecedor é de Preenchimento Obrigatório!")
                ;

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Campo Preço é de Preenchimento Obrigatório!")
                .GreaterThan(0)
                .WithMessage("Campo Preço Precisa Ser Maior que Zero!")
                ;

            RuleFor(x => x.AquisitionDate)
                .NotEmpty()
                .WithMessage("Campo Data de Aquisição é de Preenchimento Obrigatório!")
                ;

            RuleFor(x => x.ExpirationDate)
                .NotEmpty()
                .WithMessage("Campo Data de Vencimento é de Preenchimento Obrigatório!")
                ;

            RuleFor(x => x.Description)
                .MaximumLength(255)
                .WithMessage("Campo Descrição Deve Possuir de um Tamanho Máximo de 255 Caracteres");

        }
    }
}
