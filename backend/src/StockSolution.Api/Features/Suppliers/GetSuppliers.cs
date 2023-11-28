using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Suppliers;

public record GetSuppliersQuery : IRequest<List<GetSuppliersResponse>>;
public record GetSuppliersResponse(int Id, string Code, string TradingName, string CNPJ);

public sealed class GetSuppliersEndpoint : Endpoint<GetSuppliersQuery, List<GetSuppliersResponse>>
{
    private readonly ISender _mediator;

    public GetSuppliersEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/suppliers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetSuppliersQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, List<GetSuppliersResponse>>
{
    private readonly AppDbContext _context;

    public GetSuppliersQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetSuppliersResponse>> Handle(GetSuppliersQuery req, CancellationToken ct)
    {
        return await _context.Suppliers.AsNoTracking()
                    .Select(x => new GetSuppliersResponse(x.Id, x.Code, x.TradingName, x.CNPJ))
                    .ToListAsync(ct);
    }
}