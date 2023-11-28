using Microsoft.EntityFrameworkCore;
using NodaTime;
using StockSolution.Api.Common.Exceptions;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Products;

public record CreateProductCommand(
    string Name,
    string Code,
    decimal Quantity,
    int SupplierId,
    decimal Price,
    int? CategoryId,
    Instant AcquisitionDate,
    Instant ExpirationDate,
    string? Description) : IRequest<CreateProductResponse>;

public record CreateProductResponse(
    int Id,
    string Name,
    string Code,
    decimal Quantity,
    int SupplierId,
    decimal Price,
    int? CategoryId,
    Instant AcquisitionDate,
    Instant ExpirationDate,
    string? Description);

public sealed class CreateProductEndpoint : Endpoint<CreateProductCommand, CreateProductResponse>
{
    private readonly ISender _mediator;

    public CreateProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post();
        Group<ProductsGroup>();
 
        Description(c => c
            .Accepts<CreateProductCommand>("application/json")
            .Produces<CreateProductResponse>(201, "application/json")
            .ProducesValidationProblem()
        );
    }

    public override async Task HandleAsync(CreateProductCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly AppDbContext _context;

    public CreateProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateProductResponse> Handle(CreateProductCommand req, CancellationToken ct)
    {
        var supplierId = await _context.Suppliers.Select(x => x.Id)
            .FirstOrDefaultAsync(id => id == req.SupplierId, ct);

        if (supplierId == default)
        {
            throw new NotFoundException($"Supplier {req.SupplierId} not found!");
        }

        var categoryId = await _context.Categories.Select(x => x.Id)
            .FirstOrDefaultAsync(id => id == req.SupplierId, ct);

        if (categoryId == default)
        {
            throw new NotFoundException($"Category {req.CategoryId} not found!");
        }

        var product = new Product
        {
            Code = req.Code,
            Name = req.Name,
            Quantity = req.Quantity,
            SupplierId = supplierId,
            Price = req.Price,
            CategoryId = categoryId,
            AcquisitionDate = req.AcquisitionDate,
            ExpirationDate = req.ExpirationDate,
            Description = req.Description
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(ct);

        return new CreateProductResponse(product.Id, product.Name, product.Code, product.Quantity, supplierId,
            product.Price, product.Category?.Id, product.AcquisitionDate, product.ExpirationDate, product.Description);
    }
}