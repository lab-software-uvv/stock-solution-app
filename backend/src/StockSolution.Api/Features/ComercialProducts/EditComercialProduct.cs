using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Common.Exceptions;

namespace StockSolution.Api.Features.ComercialProducts;

public record EditComercialProductCommand(int Id, string Name, string Code, string? Description, decimal Price)
    : IRequest<EditComercialProductResponse>;

public record EditComercialProductResponse(int Id, string Name, string Code, string? Description, decimal Price);

public sealed class EditComercialProductEndpoint : Endpoint<EditComercialProductCommand, EditComercialProductResponse>
{
    private readonly ISender _mediator;

    public EditComercialProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Put("{id}");
        Group<ComercialProductsGroup>();
        Description(c => c
            .Accepts<EditComercialProductCommand>("application/json")
            .Produces<EditComercialProductResponse>(200, "application/json")
            .ProducesValidationProblem()
        );
    }

    public override async Task HandleAsync(EditComercialProductCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class
    EditComercialProductCommandHandler : IRequestHandler<EditComercialProductCommand, EditComercialProductResponse>
{
    private readonly AppDbContext _context;

    public EditComercialProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EditComercialProductResponse> Handle(EditComercialProductCommand req, CancellationToken ct)
    {
        var entity = await _context.ComercialProducts.FirstOrDefaultAsync(f => f.Id == req.Id, ct) ??
                     throw new NotFoundException($"Produto Comercial {req.Id} Não Encontrado!");

        entity.Name = req.Name;
        entity.Description = req.Description;
        entity.Price = req.Price!;

        await _context.SaveChangesAsync(ct);

        return new EditComercialProductResponse(entity.Id, entity.Name, entity.Code, entity.Description, entity.Price);
    }
}