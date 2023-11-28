using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record AddSoldProductRequest(int saleId, int productId, decimal quantity, decimal value) : IRequest<AddSoldProductResponse>;

public sealed class AddSoldProductEndpoint : Endpoint<AddSoldProductRequest, AddSoldProductResponse>
{
    private readonly ISender _mediator;

    public AddSoldProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/sales/{saleId}/product/{productId}");
        AllowAnonymous();
        Description(c => c
            .Accepts<AddSoldProductRequest>("application/json")
            .Produces<AddSoldProductResponse>(200, "application/json")
            .ProducesValidationProblem(400)
        );
    }

    public override async Task HandleAsync(AddSoldProductRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class AddSoldProductCommandHandler : IRequestHandler<AddSoldProductRequest, AddSoldProductResponse>
{
    private readonly AppDbContext _context;

    public AddSoldProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AddSoldProductResponse> Handle(AddSoldProductRequest request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.FirstOrDefaultAsync(u => u.Id == request.saleId) ?? throw new Exception($"Venda {request.saleId} Não Encontrada!", new KeyNotFoundException());

        if (sale.Status != SaleStatusEnum.InElaboration)
            throw new Exception("Não é possível adicionar um produto a uma venda que não está em elaboração.");
        
        var product = await _context.Products.FirstOrDefaultAsync(f => f.Id == request.productId, cancellationToken) ?? throw new Exception($"Produto {request.productId} Não Encontrado!", new KeyNotFoundException());

        if (product.Quantity < request.quantity)
            throw new Exception("Produto com quantidade insuficiente em estoque");

        product.Quantity -= request.quantity;
        
        SaleProduct soldProduct = new(sale.Id, product.Id, request.quantity, request.value);
        _context.Add(soldProduct);
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);

        return new AddSoldProductResponse(soldProduct.Id, soldProduct.SaleId, soldProduct.ProductId, soldProduct.Quantity, soldProduct.Value);
    }
}

public record AddSoldProductResponse(int id, int saleId, int productId, decimal quantity, decimal value);