using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Features.Products;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record GetSalesQuery() : IRequest<List<GetSalesResponse>>;

public sealed class GetProductsEndpoint : Endpoint<GetSalesQuery, List<GetSalesResponse>>
{
    private readonly ISender _mediator;

    public GetProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/sales");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetSalesQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, List<GetSalesResponse>>
{
    private readonly AppDbContext _context;

    public GetSalesQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetSalesResponse>> Handle(GetSalesQuery req, CancellationToken ct)
    {
        return await _context.Sales.AsNoTracking()
            .Select(x => new GetSalesResponse(x.Id, x.SellingDate, x.TotalValue, x.PaymentMethod, x.Status))
            .ToListAsync(ct);
    }
}


public record GetSalesResponse(int id, DateTime sellingDate, decimal totalValue, PaymentMethod paymentMethod, SaleStatus status);
