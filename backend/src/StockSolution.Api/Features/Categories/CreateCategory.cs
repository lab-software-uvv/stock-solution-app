using FluentValidation;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Categories;

public record CreateCategoryRequest(string Name, string Description) : IRequest<CreateCategoryResponse>;

public sealed class CreateCategoryEndpoint : Endpoint<CreateCategoryRequest, CreateCategoryResponse>
{
    private readonly ISender _mediator;

    public CreateCategoryEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/");
        Group<CategoriesGroup>();
        Description(c => c
            .Accepts<CreateCategoryRequest>("application/json")
            .Produces<CreateCategoryResponse>(201, "application/json")
            .ProducesValidationProblem()
        );
    }

    public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public class CreateCategoryRequestValidator : Validator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
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

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
{
    private readonly AppDbContext _context;

    public CreateCategoryCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest req, CancellationToken ct)
    {
        var category = new Category(req.Name, req.Description);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync(ct);

        return new CreateCategoryResponse(category.Id, category.Name, category.Description);
    }
}

public record CreateCategoryResponse(int Id, string Name, string? Description);