using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StockSolution.Api.Features.ComercialProducts;

public record GetComercialProductsQuery : IRequest<List<GetComercialProductsResponse>>;
public record GetComercialProductsResponse(int Id, string Name, string Code, string? Description, decimal Price);

public sealed class GetComercialProductsEndpoint : Endpoint<GetComercialProductsQuery, List<GetComercialProductsResponse>>
{
    private readonly ISender _mediator;

    public GetComercialProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get();
        Group<ComercialProductsGroup>();
    }

    public override async Task HandleAsync(GetComercialProductsQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetComercialProductsQueryHandler : IRequestHandler<GetComercialProductsQuery, List<GetComercialProductsResponse>>
{
    private readonly AppDbContext _context;

    public GetComercialProductsQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<GetComercialProductsResponse>> Handle(GetComercialProductsQuery req, CancellationToken ct)
    {
        return _context.ComercialProducts
            .Select(x => new GetComercialProductsResponse(x.Id, x.Name, x.Code, x.Description, x.Price))
            .ToListAsync(ct);
    }
}