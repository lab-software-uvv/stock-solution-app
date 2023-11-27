using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Categories;

public record DeleteCategory(int Id) : IRequest;

public sealed class DeleteCategoryEndpoint : Endpoint<DeleteCategory>
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

    public override async Task HandleAsync(DeleteCategory req, CancellationToken ct)
    {
        await _mediator.Send(req);
        await SendOkAsync();
    }
}

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategory>
{
    private readonly AppDbContext _context;

    public DeleteCategoryCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCategory req, CancellationToken ct)
    {
        var produtosComCategoria = await _context.Products.Where(p => p.CategoryId == req.Id).ToListAsync(ct);
        if (produtosComCategoria.Any())
        {
            produtosComCategoria.ForEach(p => p.CategoryId = null);
            await _context.SaveChangesAsync(ct);
        }


        await _context.Categories.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);
    }
}