using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Users;

public record GetUsersQuery : IRequest<List<GetUsersResponse>>;
public record GetUsersResponse(int Id, string Name, int Role);

public sealed class GetUsersEndpoint : Endpoint<GetUsersQuery, List<GetUsersResponse>>
{
    private readonly ISender _mediator;

    public GetUsersEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/users");
    }

    public override async Task HandleAsync(GetUsersQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<GetUsersResponse>>
{
    private readonly AppDbContext _context;

    public GetUsersQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<GetUsersResponse>> Handle(GetUsersQuery req, CancellationToken ct)
    {
        return _context.Users
            .Select(x => new GetUsersResponse(x.Id, x.Name, x.Role!.Id))
            .ToListAsync(ct);
    }
}