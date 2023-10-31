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
        Post("/categories");
        AllowAnonymous();
        Description(c => c
                    .Accepts<CreateCategoryRequest>("application/json")
                    .Produces<CreateCategoryResponse>(201, "application/json")
                    .ProducesValidationProblem(400)
                    );
    }

    public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
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