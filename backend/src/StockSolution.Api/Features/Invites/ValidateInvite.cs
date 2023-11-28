using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Common;
using StockSolution.Api.Common.Exceptions;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Invites;

public record ValidateInviteQuery(Guid Id, string? Email = null) : IRequest<Invite>;

public sealed class ValidateInviteQueryValidator : Validator<ValidateInviteQuery>
{
    public ValidateInviteQueryValidator()
    {
        RuleFor(x => x.Id)
            .Must(Validations.BeValidGuid);
    }
}

public sealed class ValidateInviteEndpoint : Endpoint<ValidateInviteQuery>
{
    private readonly ISender _mediator;

    public ValidateInviteEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("validate/{id}");
        
        Group<InvitesGroup>();
    }

    public override async Task HandleAsync(ValidateInviteQuery req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendOkAsync();
    }
}

public sealed class ValidateInviteQueryHandler : IRequestHandler<ValidateInviteQuery, Invite>
{
    private readonly AppDbContext _context;

    public ValidateInviteQueryHandler(AppDbContext context)
        => _context = context;

    public async Task<Invite> Handle(ValidateInviteQuery req, CancellationToken ct)
    {
        var invite = await _context.Invites
            .FirstOrDefaultAsync(x => x.Id == req.Id && x.UserId == null, ct);

        if (invite is null)
        {
            throw new NotFoundException("Convite não encontrado.");
        }

        if (req.Email is not null && invite.Email != req.Email)
        {
            throw new BadRequestException("Esse convite não é para este email.");
        }

        return invite;
    }
}