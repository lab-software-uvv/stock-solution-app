using Microsoft.EntityFrameworkCore;
namespace StockSolution.Api.Features.ProductsComercialProduct;

public record DeleteProductComercialProduct(int Id) : IRequest;

public sealed class DeleteProductComercialProductEndpoint : Endpoint<DeleteProductComercialProduct>
{
    private readonly ISender _mediator;

    public DeleteProductComercialProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/comercial-products/{comercialProductId}/products/{id}");
        
        Description(c => c
                    .Produces(200)
                    .ProducesProblem(404)
                    );
    }

    public override async Task HandleAsync(DeleteProductComercialProduct req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteProductComercialProductCommandHandler : IRequestHandler<DeleteProductComercialProduct>
{
    private readonly AppDbContext _context;

    public DeleteProductComercialProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductComercialProduct req, CancellationToken ct)
    {
        await _context.ProductComercialProducts.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);

        return;
    }
}
