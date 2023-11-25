using Microsoft.EntityFrameworkCore;
namespace StockSolution.Api.Features.ComercialProducts;

public record DeleteComercialProducts(int Id) : IRequest;

public sealed class DeleteComercialProductsEndpoint : Endpoint<DeleteComercialProducts>
{
    private readonly ISender _mediator;

    public DeleteComercialProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/comercial-products/{id}");
        AllowAnonymous();
        Description(c => c
                    .Produces(200)
                    .ProducesProblem(404)
                    );
    }

    public override async Task HandleAsync(DeleteComercialProducts req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteComercialProductsCommandHandler : IRequestHandler<DeleteComercialProducts>
{
    private readonly AppDbContext _context;

    public DeleteComercialProductsCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteComercialProducts req, CancellationToken ct)
    {
        await _context.ComercialProducts.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);

        return;
    }
}
