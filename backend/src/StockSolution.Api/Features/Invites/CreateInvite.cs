using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Common.Exceptions;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Invites;

public record CreateInviteCommand(string Email) : IRequest<CreateInviteResponse>;
public record CreateInviteResponse(Guid Id);

public sealed class CreateInviteEndpoint : Endpoint<CreateInviteCommand, CreateInviteResponse>
{
    private readonly ISender _mediator;

    public CreateInviteEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/");
        Group<InvitesGroup>();
        Description(c => c
            .Accepts<CreateInviteCommand>("application/json")
            .Produces<CreateInviteResponse>(201, "application/json")
            .ProducesValidationProblem()
        );
    }

    public override async Task HandleAsync(CreateInviteCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class CreateInviteRequestValidator : Validator<CreateInviteCommand>
{
    public CreateInviteRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Campo Email é de Preenchimento Obrigatório!")
            .EmailAddress();
    }
}

public sealed class CreateInviteCommandHandler : IRequestHandler<CreateInviteCommand, CreateInviteResponse>
{
    private readonly AppDbContext _context;

    public CreateInviteCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateInviteResponse> Handle(CreateInviteCommand req, CancellationToken ct)
    {
        var email = req.Email.ToLower();
        
        if (await _context.Invites.AnyAsync(x => x.Email == email, ct))
        {
            throw new BadRequestException("Invite already exists.");
        }
        
        if (await _context.Users.AnyAsync(x => x.Email == email, ct))
        {
            throw new BadRequestException("User already exists.");
        }
        
        var invite = new Invite
        {
            Email = email,
            RoleId = 1 // TODO: Refatorar, Admin
        };
        
        _context.Invites.Add(invite);
        await _context.SaveChangesAsync(ct);

        return new CreateInviteResponse(invite.Id);
    }
}