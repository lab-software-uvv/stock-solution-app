using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record CancelSaleRequest(int id) : IRequest;

public sealed class CancelSaleEndpoint  : Endpoint<CancelSaleRequest>
{
    private readonly ISender _mediator;

    public CancelSaleEndpoint (ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Put("/sales/{id}/cancel");
        AllowAnonymous();
        Description(c => c
            .Accepts<CancelSaleRequest>()
            .Produces(200)
            .ProducesProblem(400)
        );
    }

    public override async Task HandleAsync(CancelSaleRequest req, CancellationToken ct)
    {        
        await _mediator.Send(req);
        await SendOkAsync();
    }
}

public sealed class CancelSaleCommand : IRequestHandler<CancelSaleRequest>
{
    private readonly AppDbContext _context;

    public CancelSaleCommand (AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CancelSaleRequest req, CancellationToken ct)
    {
        var sale = await _context.Sales.Include(s => s.ComercialProducts)
                       .ThenInclude(saleComercialProduct => saleComercialProduct.ComercialProduct)
                       .ThenInclude(comercialProduct => comercialProduct.ProductComercialProduct)
                       .ThenInclude(productComercialProduct => productComercialProduct.Product)
                       .Include(s => s.Products)
                         .FirstOrDefaultAsync(f => f.Id == req.id, ct) 
                     ?? throw new Exception($"Venda {req.id} Não Encontrada!", new KeyNotFoundException());

        if (sale.Status != SaleStatusEnum.InElaboration)
            throw new Exception("Só é permitido cancelar vendas que estão em elaboração.");
        
        foreach (var soldProduct in sale.Products)
        {
            var product = await _context.Products.FirstAsync(d => d.Id == soldProduct.ProductId, ct);
            product.Quantity += soldProduct.Quantity; 
            var productToRemove = sale.Products.First(p => p.ProductId == soldProduct.ProductId);
            sale.Products.Remove(productToRemove);
        }

        foreach (var comercialProduct in sale.ComercialProducts)
        {
            foreach (var productComercialProduct in comercialProduct.ComercialProduct.ProductComercialProduct)
            {
                var product = productComercialProduct.Product;
                product.Quantity += productComercialProduct.Quantity * comercialProduct.Quantity;
            }
            var comercialProductToRemove = sale.ComercialProducts.First(p => p.ComercialProductId == comercialProduct.ComercialProductId);
            sale.ComercialProducts.Remove(comercialProductToRemove);
        }
        
        sale.Status = SaleStatusEnum.Canceled;

        await _context.SaveChangesAsync(ct);
    }
}