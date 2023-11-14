using Microsoft.EntityFrameworkCore;
namespace StockSolution.Api.Features.Products;

public record DeleteProducts(int Id) : IRequest;

public sealed class DeleteProductsEndpoint : Endpoint<DeleteProducts>
{
    private readonly ISender _mediator;

    public DeleteProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/products/{id}");
        AllowAnonymous();
        Description(c => c
                    .Produces(200)
                    .ProducesProblem(404)
                    );
    }

    public override async Task HandleAsync(DeleteProducts req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteProductsCommandHandler : IRequestHandler<DeleteProducts>
{
    private readonly AppDbContext _context;

    public DeleteProductsCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProducts req, CancellationToken ct)
    {
        await _context.Products.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);

        return;
    }
}
