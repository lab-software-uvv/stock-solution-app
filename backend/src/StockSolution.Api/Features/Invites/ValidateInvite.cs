using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Common;
using StockSolution.Api.Common.Exceptions;
using StockSolution.Api.Persistence.Entities;
using StockSolution.Api.Persistence.Queries;

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
        AllowAnonymous();
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
        var invite = await _context.Invites.GetAvailableById(req.Id);

        if (invite is null)
        {
            throw new NotFoundException("Invite not found.");
        }

        if (req.Email is not null && invite.Email != req.Email)
        {
            throw new BadRequestException("The invite is not for this email.");
        }

        return invite;
    }
}