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
        // 
    }

    public override async Task HandleAsync(GetCategoriesQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<GetCategoriesResponse>>
{
    private readonly AppDbContext _context;

    public GetCategoriesQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetCategoriesResponse>> Handle(GetCategoriesQuery req, CancellationToken ct)
    {
        return await _context.Categories.AsNoTracking()
                    .Select(x => new GetCategoriesResponse(x.Id, x.Name, x.Description))
                    .ToListAsync(ct);
    }
}

public record GetCategoriesResponse(int Id, string Name, string Description);