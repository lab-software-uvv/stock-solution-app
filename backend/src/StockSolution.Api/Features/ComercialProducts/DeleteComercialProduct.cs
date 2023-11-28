using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.ComercialProducts;

public record DeleteComercialProduct(int Id) : IRequest;

public sealed class DeleteComercialProductEndpoint : Endpoint<DeleteComercialProduct>
{
    private readonly ISender _mediator;

    public DeleteComercialProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("{id}");
        Group<ComercialProductsGroup>();
        Description(c => c
            .Produces(200)
            .ProducesProblem(404)
        );
    }

    public override async Task HandleAsync(DeleteComercialProduct req, CancellationToken ct)
    {
        await _mediator.Send(req);
        await SendOkAsync();
    }
}

public sealed class DeleteComercialProductCommandHandler : IRequestHandler<DeleteComercialProduct>
{
    private readonly AppDbContext _context;

    public DeleteComercialProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteComercialProduct req, CancellationToken ct)
    {
        await _context.ComercialProducts.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);
    }
}