using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record DeleteSoldComercialProductRequest(int saleId, int comercialProductId) : IRequest;

public sealed class DeleteSoldComercialProductEndpoint : Endpoint<DeleteSoldComercialProductRequest>
{
    private readonly ISender _mediator;

    public DeleteSoldComercialProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/sales/{saleId}/comercial-product/{comercialProductId}");
        AllowAnonymous();
        Description(c => c
            .Produces(200)
            .ProducesProblem(404)
        );
    }

    public override async Task HandleAsync(DeleteSoldComercialProductRequest req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteSoldComercialProductCommandHandler : IRequestHandler<DeleteSoldComercialProductRequest>
{
    private readonly AppDbContext _context;

    public DeleteSoldComercialProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSoldComercialProductRequest request, CancellationToken ct)
    {
        var sale = await _context.Sales
                       .Where(s => s.Id == request.saleId)
                       .Include(s => s.ComercialProducts)
                       .FirstOrDefaultAsync(ct) 
                   ?? throw new Exception($"Venda {request.saleId} Não Encontrada!", new KeyNotFoundException());
        
        if (sale.Status != SaleStatus.InElaboration)
            throw new Exception("Não é possível remover um produto comercial de uma venda que não está em elaboração.");

        var comercialProductToRemove = sale.ComercialProducts.FirstOrDefault(p => p.ComercialProductId == request.comercialProductId);

        if (comercialProductToRemove == null)
            throw new Exception($"Produto comercial {request.comercialProductId} não encontrado na venda {request.saleId}.");

        sale.ComercialProducts.Remove(comercialProductToRemove);
        await _context.SaveChangesAsync(ct);
        
        return;
    }
}