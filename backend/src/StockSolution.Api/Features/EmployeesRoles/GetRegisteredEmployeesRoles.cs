using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Features.Events;
namespace StockSolution.Api.Features.Employees;

public record GetRegisteredEmployeesRolesQuery : IRequest<List<GetRegisteredEmployeesRolesResponse>>;

public sealed class GetRegisteredEmployeesRolesEndpoint : Endpoint<GetRegisteredEmployeesRolesQuery, List<GetRegisteredEmployeesRolesResponse>>
{
    private readonly ISender _mediator;

    public GetRegisteredEmployeesRolesEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/available");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRegisteredEmployeesRolesQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetRegisteredEmployeesRolesHandler : IRequestHandler<GetRegisteredEmployeesRolesQuery, List<GetRegisteredEmployeesRolesResponse>>
{
    private readonly AppDbContext _context;

    public GetRegisteredEmployeesRolesHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<GetRegisteredEmployeesRolesResponse>> Handle(GetRegisteredEmployeesRolesQuery req, CancellationToken ct)
        => _context.EmployeesRoles
            .Select(x => new GetRegisteredEmployeesRolesResponse(x.Id, x.Description))
            .ToListAsync(ct);
}

public record GetRegisteredEmployeesRolesResponse(int Id, string Description);