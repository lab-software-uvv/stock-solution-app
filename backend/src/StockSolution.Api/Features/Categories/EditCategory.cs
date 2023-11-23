using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Categories;

public record EditCategoryCommand(int Id, string Name, string Description) : IRequest<EditCategoriesResponse>;

public sealed class EditCategoryEndpoint : Endpoint<EditCategoryCommand, EditCategoriesResponse>
{
    private readonly ISender _mediator;

    public EditCategoryEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Put("{id}");
        Group<CategoriesGroup>();
        Description(c => c
            .Accepts<EditCategoryCommand>("application/json")
            .Produces<EditCategoriesResponse>(200, "application/json")
            .ProducesValidationProblem()
        );
    }

    public override async Task HandleAsync(EditCategoryCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public class EditCategoryCommandValidator : Validator<EditCategoryCommand>
{
    public EditCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Campo Nome é de Preenchimento Obrigatório!")
            .MinimumLength(3)
            .WithMessage("Campo Nome Deve Possuir um Tamanho Mínimo de 3 Caracteres")
            .MaximumLength(50)
            .WithMessage("Campo Nome Deve Possuir um Tamanho Máximo de 50 Caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(255)
            .WithMessage("Campo Descrição Deve Possuir de um Tamanho Máximo de 50 Caracteres");
    }
}

public sealed class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, EditCategoriesResponse>
{
    private readonly AppDbContext _context;

    public EditCategoryCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EditCategoriesResponse> Handle(EditCategoryCommand req, CancellationToken ct)
    {
        var entity = await _context.Categories.FirstOrDefaultAsync(f => f.Id == req.Id, ct) ??
                     throw new Exception($"Categoria {req.Id} Não Encontrada!", new KeyNotFoundException());

        entity.Name = req.Name;
        entity.Description = req.Description;

        await _context.SaveChangesAsync(ct);

        return new EditCategoriesResponse(entity.Id, entity.Name, entity.Description);
    }
}

public record EditCategoriesResponse(int Id, string Name, string Description);