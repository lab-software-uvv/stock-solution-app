using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace StockSolution.Api.Features.Suppliers;

public record GetSupplierByIdQuery(int Id) : IRequest<GetSupplierByIdResponse>;

public sealed class GetSupplierByIdEndpoint : Endpoint<GetSupplierByIdQuery, GetSupplierByIdResponse>
{
    private readonly ISender _mediator;

    public GetSupplierByIdEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/suppliers/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetSupplierByIdQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, GetSupplierByIdResponse>
{
    private readonly AppDbContext _context;

    public GetSupplierByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetSupplierByIdResponse> Handle(GetSupplierByIdQuery req, CancellationToken ct)
    {
        var entity = await _context.Suppliers.AsNoTracking().FirstOrDefaultAsync(f => f.Id == req.Id, ct);

        return entity is null
            ? throw new Exception($"Fornecedor {req.Id} Não Encontrado!", new KeyNotFoundException())
            : new GetSupplierByIdResponse(entity!.Id, entity.Code, entity.TradingName, entity.CNPJ);
    }
}

public record GetSupplierByIdResponse(int Id, string Code, string TradingName, string CNPJ);