using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record AddSoldComercialProductRequest(int saleId, int comercialProductId, decimal quantity, decimal value) : IRequest<AddSoldComercialProductResponse>;

public sealed class AddSoldComercialProductEndpoint : Endpoint<AddSoldComercialProductRequest, AddSoldComercialProductResponse>
{
    private readonly ISender _mediator;

    public AddSoldComercialProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/sales/{saleId}/comercial-product/{comercialProductId}");
        AllowAnonymous();
        Description(c => c
            .Accepts<AddSoldComercialProductRequest>("application/json")
            .Produces<AddSoldComercialProductResponse>(200, "application/json")
            .ProducesValidationProblem(400)
        );
    }

    public override async Task HandleAsync(AddSoldComercialProductRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class AddSoldComercialProductCommandHandler : IRequestHandler<AddSoldComercialProductRequest, AddSoldComercialProductResponse>
{
    private readonly AppDbContext _context;

    public AddSoldComercialProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AddSoldComercialProductResponse> Handle(AddSoldComercialProductRequest request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales.FirstOrDefaultAsync(u => u.Id == request.saleId) ?? throw new Exception($"Venda {request.saleId} Não Encontrada!", new KeyNotFoundException());
       
        if (sale.Status != SaleStatusEnum.InElaboration)
            throw new Exception("Não é possível adicionar um produto a uma venda que não está em elaboração.");
        
        var comercialProduct = await _context.ComercialProducts.FirstOrDefaultAsync(f => f.Id == request.comercialProductId, cancellationToken) ?? throw new Exception($"Produto Comercial {request.comercialProductId} Não Encontrado!", new KeyNotFoundException());

        SaleComercialProduct soldComercialProduct = new(sale.Id, comercialProduct.Id, request.quantity, request.value);
        _context.Add(soldComercialProduct);
        await _context.SaveChangesAsync(cancellationToken);

        return new AddSoldComercialProductResponse(soldComercialProduct.Id, soldComercialProduct.SaleId, soldComercialProduct.ComercialProductId, soldComercialProduct.Quantity, soldComercialProduct.Value);
    }
}

public record AddSoldComercialProductResponse(int id, int saleId, int comercialProductId, decimal quantity, decimal value);