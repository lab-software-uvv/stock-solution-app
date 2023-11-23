using FluentValidation;
using StockSolution.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Auth;

public record LoginCommand(string Email, string Password) : IRequest<TokenResponse>;

public sealed class LoginEndpoint : Endpoint<LoginCommand, TokenResponse>
{
    private readonly ISender _mediator;

    public LoginEndpoint(ISender mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req, ct));
}

public sealed class LoginCommandValidator : Validator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail não é válido.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(5);
    }
}

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtGenerator _jwtGenerator;

    public LoginCommandHandler(AppDbContext context, PasswordHasher passwordHasher, JwtGenerator jwtGenerator)
        => (_context, _passwordHasher, _jwtGenerator) = (context, passwordHasher, jwtGenerator);

    public async Task<TokenResponse> Handle(LoginCommand req, CancellationToken ct)
    {
        var emailLower = req.Email.ToLower();
        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email.ToLower() == emailLower, ct);

        // TODO: Create a Guard clause class, to handle NotFound and other kinds of exceptions.
        if (user is null)
            throw new UnauthorizedAccessException();

        if (!_passwordHasher.Verify(user.PasswordHash, req.Password))
            throw new UnauthorizedAccessException();

        var jwtToken = _jwtGenerator.GenerateToken(user, user.Role!);
        return new TokenResponse(jwtToken);
    }
}