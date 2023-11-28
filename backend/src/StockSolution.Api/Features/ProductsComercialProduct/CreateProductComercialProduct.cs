using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.ProductsComercialProduct;

public record CreateProductComercialProductRequest(int ComercialProductId, int ProductId, decimal Quantity) : IRequest<CreateProductComercialProductResponse>;

public sealed class CreateProductComercialProductEndpoint : Endpoint<CreateProductComercialProductRequest, CreateProductComercialProductResponse>
{
    private readonly ISender _mediator;

    public CreateProductComercialProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/comercial-products/{comercialProductId}/products");
        AllowAnonymous();
        Description(c => c
                    .Accepts<CreateProductComercialProductRequest>("application/json")
                    .Produces<CreateProductComercialProductResponse>(201, "application/json")
                    .ProducesValidationProblem(400)
                    );
    }

    public override async Task HandleAsync(CreateProductComercialProductRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class CreateProductComercialProductCommandHandler : IRequestHandler<CreateProductComercialProductRequest, CreateProductComercialProductResponse>
{
    private readonly AppDbContext _context;

    public CreateProductComercialProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateProductComercialProductResponse> Handle(CreateProductComercialProductRequest req, CancellationToken ct)
    {

        ProductComercialProduct product = new(req.ComercialProductId, req.ProductId, req.Quantity);
        _context.ProductComercialProducts.Add(product);
        await _context.SaveChangesAsync(ct);

        return new CreateProductComercialProductResponse(product.Id, product.ComercialProductId, product.ProductId, product.Quantity);
    }
}

public record CreateProductComercialProductResponse(int Id, int ComercialProductId, int ProductId, decimal Quantity);