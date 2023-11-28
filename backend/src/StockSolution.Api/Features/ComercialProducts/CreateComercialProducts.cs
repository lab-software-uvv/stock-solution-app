using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.ComercialProductss;

public record CreateComercialProductsRequest(string Name, string Code, string? Description, decimal Price) : IRequest<CreateComercialProductsResponse>;

public sealed class CreateComercialProductsEndpoint : Endpoint<CreateComercialProductsRequest, CreateComercialProductsResponse>
{
    private readonly ISender _mediator;

    public CreateComercialProductsEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/comercial-products");
        
        Description(c => c
                    .Accepts<CreateComercialProductsRequest>("application/json")
                    .Produces<CreateComercialProductsResponse>(201, "application/json")
                    .ProducesValidationProblem(400)
                    );
    }

    public override async Task HandleAsync(CreateComercialProductsRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class CreateComercialProductsCommandHandler : IRequestHandler<CreateComercialProductsRequest, CreateComercialProductsResponse>
{
    private readonly AppDbContext _context;

    public CreateComercialProductsCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateComercialProductsResponse> Handle(CreateComercialProductsRequest req, CancellationToken ct)
    {

        ComercialProduct comercialProduct = new(req.Code, req.Name, req.Description, req.Price);
        _context.ComercialProducts.Add(comercialProduct);
        await _context.SaveChangesAsync(ct);

        return new CreateComercialProductsResponse(comercialProduct.Id, comercialProduct.Name, comercialProduct.Code, comercialProduct.Description, comercialProduct.Price);
    }
}

public record CreateComercialProductsResponse(int Id, string Name, string Code, string? Descriptiopn, decimal Price);