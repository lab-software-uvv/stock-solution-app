using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Products;

public record GetProductByIdQuery(int Id) : IRequest<GetProductByIdResponse>;

public sealed class GetProductByIdEndpoint : Endpoint<GetProductByIdQuery, GetProductByIdResponse>
{
    private readonly ISender _mediator;

    public GetProductByIdEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/products/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductByIdQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    private readonly AppDbContext _context;

    public GetProductByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery req, CancellationToken ct)
    {
        var entity = await _context.Products.AsNoTracking().Include(p => p.Supplier).FirstOrDefaultAsync(f => f.Id == req.Id, ct);

        return entity is null
            ? throw new Exception($"Produto {req.Id} Não Encontrado!", new KeyNotFoundException())
            : new GetProductByIdResponse(entity!.Id, entity.Name, entity.Code, entity.Quantity, entity.Supplier.Code, entity.Price, entity.Category?.Name, entity.AquisitionDate, entity.ExpirationDate, entity.Description);
    }
}

public record GetProductByIdResponse(int Id, string Name, string Code, decimal Quantity, string SupplierCode, decimal Price, string? CategoryName, DateTime AquisitionDate, DateTime ExpirationDate, string? Description);