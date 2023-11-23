using EntityFramework.Exceptions.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Services;
using NodaTime;
using StockSolution.Api.Common;
using StockSolution.Api.Common.Exceptions;
using StockSolution.Api.Features.Invites;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Auth;

public record RegisterCommand(string Name, string Cpf, LocalDate BirthDate, string Email, string Password,
    Guid Invite) : IRequest<TokenResponse>;

public sealed class RegisterEndpoint : Endpoint<RegisterCommand, TokenResponse>
{
    private readonly ISender _mediator;

    public RegisterEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req, ct));
}

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(6).WithMessage("O nome deve ter pelo menos 6 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail não é válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(5).WithMessage("A senha deve ter pelo menos 5 caracteres.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(Validations.BeValidCpf).WithMessage("O CPF inserido não é válido.");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .Must(BeAValidDate).WithMessage("A data de nascimento não é válida.")
            .Must(BeAPastDate).WithMessage("A data de nascimento deve ser no passado.");

        RuleFor(x => x.Invite)
            .NotEmpty().WithMessage("O código de convite é obrigatório.")
            .Must(Validations.BeValidGuid).WithMessage("O código de convite não é válido.");
    }

    private static bool BeAValidDate(LocalDate date)
    {
        var minDate = new LocalDate(1900, 1, 1);
        var maxDate = LocalDate.FromDateTime(DateTime.UtcNow);
        return date >= minDate && date <= maxDate;
    }

    private static bool BeAPastDate(LocalDate date)
    {
        return date < LocalDate.FromDateTime(DateTime.Now);
    }
}


public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, TokenResponse>
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtGenerator _jwtGenerator;
    private readonly ISender _sender;

    public RegisterCommandHandler(AppDbContext context, PasswordHasher passwordHasher, JwtGenerator jwtGenerator,
        ISender sender)
        => (_context, _passwordHasher, _jwtGenerator, _sender) = (context, passwordHasher, jwtGenerator, sender);

    public async Task<TokenResponse> Handle(RegisterCommand req, CancellationToken ct)
    {
        var emailLower = req.Email.ToLower();
        var invite = await _sender.Send(new ValidateInviteQuery(req.Invite, emailLower), ct);
        
        var cleanCpf = req.Cpf.Replace(".", "").Replace("-", "");
        var user = new User
        {
            Name = req.Name,
            Email = req.Email,
            Cpf = cleanCpf,
            BirthDate = req.BirthDate,
            RoleId = invite.RoleId,
            PasswordHash = _passwordHasher.Hash(req.Password)
        };
        invite.User = user;
        _context.Users.Add(user);
        
        try
        {
            await _context.SaveChangesAsync(ct);
        }
        catch (UniqueConstraintException)
        {
            throw new ConflictException("This email or cpf is already in use.");
        }

        var userRole = await _context.Roles.FirstAsync(x => x.Id == user.RoleId, ct);
        var jwtToken = _jwtGenerator.GenerateToken(user, userRole);
        return new TokenResponse(jwtToken);
    }
}