using StockSolution.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Auth;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;

public sealed class LoginEndpoint : Endpoint<LoginCommand, LoginResponse>
{
    private readonly ISender _mediator;

    public LoginEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req, ct));
}

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly AppDbContext _context;
    private PasswordHasher _passwordHasher;

    public LoginCommandHandler(AppDbContext context, PasswordHasher passwordHasher)
        => (_context, _passwordHasher) = (context, passwordHasher);

    public async Task<LoginResponse> Handle(LoginCommand req, CancellationToken ct)
    {
        var lowercaseUsername = req.Username.ToLower();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == lowercaseUsername, ct);

        // TODO: Create a Guard clause class, to handle NotFound and other kinds of exceptions.
        if (user is null)
            throw new UnauthorizedAccessException();
        
        if (!_passwordHasher.Verify(user.PasswordHash, req.Password))
            throw new UnauthorizedAccessException();
        
        // TODO: Generate Token and Access Token
        return new LoginResponse();
    }
}

public record LoginResponse();