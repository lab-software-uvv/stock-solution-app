using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Suppliers;

public record CreateSupplierRequest(string Code, string TradingName, string CNPJ) : IRequest<CreateSupplierResponse>;
public record CreateSupplierResponse(int Id, string Code, string TradingName, string CNPJ);

public sealed class CreateSupplierEndpoint : Endpoint<CreateSupplierRequest, CreateSupplierResponse>
{
    private readonly ISender _mediator;

    public CreateSupplierEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/suppliers");
        AllowAnonymous();
        Description(c => c
                    .Accepts<CreateSupplierRequest>("application/json")
                    .Produces<CreateSupplierResponse>(201, "application/json")
                    .ProducesValidationProblem(400)
                    );
    }

    public override async Task HandleAsync(CreateSupplierRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierRequest, CreateSupplierResponse>
{
    private readonly AppDbContext _context;

    public CreateSupplierCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateSupplierResponse> Handle(CreateSupplierRequest req, CancellationToken ct)
    {
        var supplier = new Supplier
        {
            Code = req.Code,
            TradingName = req.TradingName,
            CNPJ = req.CNPJ
        };
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync(ct);

        return new CreateSupplierResponse(supplier.Id, supplier.Code, supplier.TradingName, supplier.CNPJ);
    }
}