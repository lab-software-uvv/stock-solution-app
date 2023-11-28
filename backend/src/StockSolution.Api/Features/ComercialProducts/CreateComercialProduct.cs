using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.ComercialProducts;

public record CreateComercialProductRequest(string Name, string Code, string? Description, decimal Price) : IRequest<CreateComercialProductResponse>;
public record CreateComercialProductResponse(int Id, string Name, string Code, string? Descriptiopn, decimal Price);

public sealed class CreateComercialProductEndpoint : Endpoint<CreateComercialProductRequest, CreateComercialProductResponse>
{
    private readonly ISender _mediator;

    public CreateComercialProductEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post();
        Group<ComercialProductsGroup>();
        Description(c => c
                    .Accepts<CreateComercialProductRequest>("application/json")
                    .Produces<CreateComercialProductResponse>(201, "application/json")
                    .ProducesValidationProblem(400)
                    );
    }

    public override async Task HandleAsync(CreateComercialProductRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class CreateComercialProductCommandHandler : IRequestHandler<CreateComercialProductRequest, CreateComercialProductResponse>
{
    private readonly AppDbContext _context;

    public CreateComercialProductCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateComercialProductResponse> Handle(CreateComercialProductRequest req, CancellationToken ct)
    {
        var entity = new ComercialProduct
        {
            Code = req.Code,
            Name = req.Name,
            Price = req.Price,
            Description = req.Description
        };
        _context.ComercialProducts.Add(entity);
        await _context.SaveChangesAsync(ct);

        return new CreateComercialProductResponse(entity.Id, entity.Name, entity.Code, entity.Description, entity.Price);
    }
}