using Microsoft.EntityFrameworkCore;
namespace StockSolution.Api.Features.Products;

public record DeleteProductCommand(int Id) : IRequest;

public sealed class DeleteProductsEndpoint : Endpoint<DeleteProductCommand>
{
    private readonly ISender _mediator;

    public DeleteProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("{id}");
        Group<ProductsGroup>();
        
        Description(c => c
                    .Produces(200)
                    .ProducesProblem(404)
                    );
    }

    public override async Task HandleAsync(DeleteProductCommand req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteProductsCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly AppDbContext _context;

    public DeleteProductsCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductCommand req, CancellationToken ct)
    {
        await _context.Products.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);
    }
}
