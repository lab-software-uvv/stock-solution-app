using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace StockSolution.Api.Features.Products;

public record GetProductsQuery : IRequest<List<GetProductsResponse>>;
public record GetProductsResponse(int Id, string Name, string Code, decimal Quantity, string SupplierCode, decimal Price, string? CategoryName, Instant AquisitionDate, Instant ExpirationDate, string? Description);

public sealed class GetProductsEndpoint : Endpoint<GetProductsQuery, List<GetProductsResponse>>
{
    private readonly ISender _mediator;

    public GetProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductsQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductsResponse>>
{
    private readonly AppDbContext _context;

    public GetProductsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetProductsResponse>> Handle(GetProductsQuery req, CancellationToken ct)
    {
        return await _context.Products
                    .Select(x => new GetProductsResponse(x.Id, x.Name, x.Code, x.Quantity, x.Supplier!.Code, x.Price, x.Category!.Name, x.AcquisitionDate, x.ExpirationDate, x.Description))
                    .ToListAsync(ct);
    }
}