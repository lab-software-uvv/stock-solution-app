using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Common.Exceptions;

namespace StockSolution.Api.Features.ComercialProducts;

public record GetComercialProductByIdQuery(int Id) : IRequest<GetComercialProductByIdResponse>;
public record GetComercialProductByIdResponse(int Id, string Name, string Code, string? Description, decimal Price);

public sealed class GetComercialProductByIdEndpoint : Endpoint<GetComercialProductByIdQuery, GetComercialProductByIdResponse>
{
    private readonly ISender _mediator;

    public GetComercialProductByIdEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("{id}");
        Group<ComercialProductsGroup>();
    }

    public override async Task HandleAsync(GetComercialProductByIdQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetComercialProductByIdQueryHandler : IRequestHandler<GetComercialProductByIdQuery, GetComercialProductByIdResponse>
{
    private readonly AppDbContext _context;

    public GetComercialProductByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetComercialProductByIdResponse> Handle(GetComercialProductByIdQuery req, CancellationToken ct)
    {
        var entity = await _context.ComercialProducts.AsNoTracking().FirstOrDefaultAsync(f => f.Id == req.Id, ct);

        return entity is null
            ? throw new NotFoundException($"Produto {req.Id} Não Encontrado!")
            : new GetComercialProductByIdResponse(entity.Id, entity.Name, entity.Code, entity.Description, entity.Price);
    }
}