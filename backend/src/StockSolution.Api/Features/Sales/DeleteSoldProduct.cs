using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record DeleteSoldProductRequest(int saleId, int productId) : IRequest;

public sealed class DeleteSoldProductEndpoint : Endpoint<DeleteSoldProductRequest>
{
    private readonly ISender _mediator;

    public DeleteSoldProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/sales/{saleId}/product/{productId}");
        AllowAnonymous();
        Description(c => c
            .Produces(200)
            .ProducesProblem(404)
        );
    }

    public override async Task HandleAsync(DeleteSoldProductRequest req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteSoldProductCommandHandler : IRequestHandler<DeleteSoldProductRequest>
{
    private readonly AppDbContext _context;

    public DeleteSoldProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSoldProductRequest request, CancellationToken ct)
    {
        var sale = await _context.Sales
                       .Where(s => s.Id == request.saleId)
                       .Include(s => s.Products)
                       .FirstOrDefaultAsync(ct) 
                   ?? throw new Exception($"Venda {request.saleId} Não Encontrada!", new KeyNotFoundException());
        
        if (sale.Status != SaleStatusEnum.InElaboration)
            throw new Exception("Não é possível remover um produto de uma venda que não está em elaboração.");

        var productToRemove = sale.Products.FirstOrDefault(p => p.ProductId == request.productId);

        if (productToRemove == null)
            throw new Exception($"Produto {request.productId} não encontrado na venda {request.saleId}.");

        sale.Products.Remove(productToRemove);
        await _context.SaveChangesAsync(ct);
        
        return;
    }
}