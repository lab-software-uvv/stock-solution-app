using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Sales;

public record GetSoldComercialProductsQuery(int saleId) : IRequest<List<GetSoldComercialProductsResponse>>;

public sealed class GetSoldComercialProductsEndpoint : Endpoint<GetSoldComercialProductsQuery, List<GetSoldComercialProductsResponse>>
{
    private readonly ISender _mediator;

    public GetSoldComercialProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/sales/{saleId}/comercial-products");
        
    }

    public override async Task HandleAsync(GetSoldComercialProductsQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetSoldComercialProductsQueryHandler : IRequestHandler<GetSoldComercialProductsQuery, List<GetSoldComercialProductsResponse>>
{
    private readonly AppDbContext _context;

    public GetSoldComercialProductsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetSoldComercialProductsResponse>> Handle(GetSoldComercialProductsQuery req, CancellationToken ct)
    {
        return await _context.Sales.AsNoTracking()
            .Where(s => s.Id == req.saleId)
            .SelectMany(s => s.ComercialProducts.Select(sp => new GetSoldComercialProductsResponse(
                s.Id,
                sp.ComercialProductId,
                sp.ComercialProduct.Name,
                sp.Quantity,
                sp.Value
            )))
            .ToListAsync(ct);
    }
}


public record GetSoldComercialProductsResponse(int id, int comercialProductId, string comercialProductName, decimal quantity, decimal value);