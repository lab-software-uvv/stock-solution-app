using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StockSolution.Api.Features.ProductsComercialProduct;

public record GetProductComercialProductQuery() : IRequest<List<GetProductComercialProductResponse>>;

public sealed class GetProductComercialProductEndpoint : Endpoint<GetProductComercialProductQuery, List<GetProductComercialProductResponse>>
{
    private readonly ISender _mediator;

    public GetProductComercialProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/comercial-products/{comercialProductId}/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductComercialProductQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetProductComercialProductQueryHandler : IRequestHandler<GetProductComercialProductQuery, List<GetProductComercialProductResponse>>
{
    private readonly AppDbContext _context;

    public GetProductComercialProductQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetProductComercialProductResponse>> Handle(GetProductComercialProductQuery req, CancellationToken ct)
    {
        return await _context.ProductComercialProducts.AsNoTracking()
                    .Select(entity => new GetProductComercialProductResponse(entity!.Id, entity.ComercialProductId, entity.ProductId, entity.Quantity))
                    .ToListAsync(ct);
    }
}

public record GetProductComercialProductResponse(int Id, int ComercialProductId, int ProductId, decimal Quantity);