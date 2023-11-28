using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Products;

public record EditProductCommand(int Id, string Name, string Code, decimal Quantity, int SupplierId, decimal Price, int? CategoryId, DateTime AquisitionDate, DateTime ExpirationDate, string? Description) : IRequest<EditProductsResponse>;

public sealed class EditProductEndpoint  : Endpoint<EditProductCommand, EditProductsResponse>
{
    private readonly ISender _mediator;

    public EditProductEndpoint (ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Put("/products/{id}");
        
        Description(c => c
                   .Accepts<EditProductCommand>("application/json")
                   .Produces<EditProductsResponse>(200, "application/json")
                   .ProducesValidationProblem(400)
                   );
    }

    public override async Task HandleAsync(EditProductCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class EditProductCommandHandler  : IRequestHandler<EditProductCommand, EditProductsResponse>
{
    private readonly AppDbContext _context;

    public EditProductCommandHandler (AppDbContext context)
    {
        _context = context;
    }

    public async Task<EditProductsResponse> Handle(EditProductCommand req, CancellationToken ct)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(f => f.Id == req.Id, ct) ?? throw new Exception($"Produto {req.Id} Não Encontrado!", new KeyNotFoundException());
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(f => f.Id == req.SupplierId, ct);
        var category = await _context.Categories.FirstOrDefaultAsync(f => f.Id == req.CategoryId, ct);

        entity.Name = req.Name;
        entity.Quantity = req.Quantity;
        entity.Supplier = supplier!;
        entity.Price = req.Price;
        entity.Category = category;
        entity.AquisitionDate = req.AquisitionDate;
        entity.ExpirationDate = req.ExpirationDate;
        entity.Description = req.Description;

        await _context.SaveChangesAsync(ct);

        return new EditProductsResponse(entity.Id, entity.Name, entity.Code, entity.Quantity, entity.Supplier.Code, entity.Price, entity.Category?.Name, entity.AquisitionDate, entity.ExpirationDate, entity.Description);

    }
}

public record EditProductsResponse(int Id, string Name, string Code, decimal Quantity, string SupplierCode, decimal Price, string? CategoryName, DateTime AquisitionDate, DateTime ExpirationDate, string? Description);