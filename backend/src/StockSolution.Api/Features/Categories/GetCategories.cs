using Microsoft.EntityFrameworkCore;
namespace StockSolution.Api.Features.Categories;

public record GetCategoriesQuery() : IRequest<List<GetCategoriesResponse>>;

public sealed class GetCategoriesEndpoint : Endpoint<GetCategoriesQuery, List<GetCategoriesResponse>>
{
    private readonly ISender _mediator;

    public GetCategoriesEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/categories");
    }

    public override async Task HandleAsync(GetCategoriesQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<GetCategoriesResponse>>
{
    private readonly AppDbContext _context;

    public GetCategoriesHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<GetCategoriesResponse>> Handle(GetCategoriesQuery req, CancellationToken ct)
    {
        return _context.Categories
                    .Select(x => new GetCategoriesResponse(x.Name, x.Description))
                    .ToListAsync(ct);
    }
}

public record GetCategoriesResponse(string name, string description);