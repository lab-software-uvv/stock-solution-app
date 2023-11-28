using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.ProductsComercialProduct;

public record GetProductComercialProductByIdQuery(int Id) : IRequest<GetProductComercialProductByIdResponse>;

public sealed class GetProductComercialProductByIdEndpoint : Endpoint<GetProductComercialProductByIdQuery, GetProductComercialProductByIdResponse>
{
    private readonly ISender _mediator;

    public GetProductComercialProductByIdEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/comercial-products/{comercialProductid}/product/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductComercialProductByIdQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetProductComercialProductByIdQueryHandler : IRequestHandler<GetProductComercialProductByIdQuery, GetProductComercialProductByIdResponse>
{
    private readonly AppDbContext _context;

    public GetProductComercialProductByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetProductComercialProductByIdResponse> Handle(GetProductComercialProductByIdQuery req, CancellationToken ct)
    {
        var entity = await _context.ProductComercialProducts.AsNoTracking().FirstOrDefaultAsync(f => f.Id == req.Id, ct);

        return entity is null
            ? throw new Exception($"Produto {req.Id} Não Encontrado!", new KeyNotFoundException())
            : new GetProductComercialProductByIdResponse(entity!.Id, entity.ComercialProductId, entity.ProductId, entity.Quantity);
    }
}

public record GetProductComercialProductByIdResponse(int Id, int ComercialProductId, int ProductId, decimal Quantity);