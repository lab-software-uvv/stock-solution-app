using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Categories;

public record DeleteCategoryCommand(int Id) : IRequest;

public sealed class DeleteCategoryEndpoint : Endpoint<DeleteCategoryCommand>
{
    private readonly ISender _mediator;

    public DeleteCategoryEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("{id}");
        Group<CategoriesGroup>();
        Description(c => c
            .Produces(200)
            .ProducesProblem(404)
        );
    }

    public override async Task HandleAsync(DeleteCategoryCommand req, CancellationToken ct)
    {
        await _mediator.Send(req);
        await SendOkAsync();
    }
}

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly AppDbContext _context;

    public DeleteCategoryCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCategoryCommand req, CancellationToken ct)
    {
        var products = await _context.Products
            .Where(p => p.CategoryId == req.Id)
            .ToListAsync(ct);

        if (products.Any())
        {
            foreach (var product in products)
            {
                product.CategoryId = null;
            }

            await _context.SaveChangesAsync(ct);
        }

        await _context.Categories.Where(c => c.Id == req.Id)
            .ExecuteDeleteAsync(ct);
    }
}