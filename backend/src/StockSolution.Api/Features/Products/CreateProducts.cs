using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Products;

public record CreateProductRequest(string Name, string Code, decimal Quantity, int SupplierId, decimal Price, int? CategoryId, DateTime AquisitionDate, DateTime ExpirationDate, string? Description) : IRequest<CreateProductResponse>;

public sealed class CreateProductEndpoint : Endpoint<CreateProductRequest, CreateProductResponse>
{
    private readonly ISender _mediator;

    public CreateProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();
        Description(c => c
                    .Accepts<CreateProductRequest>("application/json")
                    .Produces<CreateProductResponse>(201, "application/json")
                    .ProducesValidationProblem(400)
                    );
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly AppDbContext _context;

    public CreateProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateProductResponse> Handle(CreateProductRequest req, CancellationToken ct)
    {
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == req.SupplierId, ct);
        var category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == req.CategoryId, ct);

        Product product = new(req.Code, req.Name, req.Description!, req.Quantity, supplier!, req.Price, category!, req.AquisitionDate, req.ExpirationDate);
        _context.Products.Add(product);
        await _context.SaveChangesAsync(ct);

        return new CreateProductResponse(product.Id, product.Name, product.Code, product.Quantity, product.Supplier.Id, product.Price, product.Category?.Id, product.AquisitionDate, product.ExpirationDate, product.Description);
    }
}

public record CreateProductResponse(int Id, string Name, string Code, decimal Quantity, int SupplierId, decimal Price, int? CategoryId, DateTime AquisitionDate, DateTime ExpirationDate, string? Description);