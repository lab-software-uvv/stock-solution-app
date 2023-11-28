using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Sales;

public record GetSoldProductsQuery(int saleId) : IRequest<List<GetSoldProductsResponse>>;

public sealed class GetSoldProductsEndpoint : Endpoint<GetSoldProductsQuery, List<GetSoldProductsResponse>>
{
    private readonly ISender _mediator;

    public GetSoldProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/sales/{saleId}/products");
        
    }

    public override async Task HandleAsync(GetSoldProductsQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetSoldProductsQueryHandler : IRequestHandler<GetSoldProductsQuery, List<GetSoldProductsResponse>>
{
    private readonly AppDbContext _context;

    public GetSoldProductsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetSoldProductsResponse>> Handle(GetSoldProductsQuery req, CancellationToken ct)
    {
        return await _context.Sales.AsNoTracking()
            .Where(s => s.Id == req.saleId)
            .SelectMany(s => s.Products.Select(sp => new GetSoldProductsResponse(
                s.Id,
                sp.ProductId,
                sp.Product.Name,
                sp.Quantity,
                sp.Value
            )))
            .ToListAsync(ct);
    }
}


public record GetSoldProductsResponse(int id, int productId, string productName, decimal quantity, decimal value);