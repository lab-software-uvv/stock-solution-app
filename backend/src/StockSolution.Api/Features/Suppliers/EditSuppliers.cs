using Microsoft.EntityFrameworkCore;

namespace StockSolution.Api.Features.Suppliers;

public record EditSupplierCommand(int Id, string Code, string TradingName, string CNPJ) : IRequest<EditSuppliersResponse>;

public sealed class EditSupplierEndpoint  : Endpoint<EditSupplierCommand, EditSuppliersResponse>
{
    private readonly ISender _mediator;

    public EditSupplierEndpoint (ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Put("/suppliers/{id}");
        AllowAnonymous();
        Description(c => c
                   .Accepts<EditSupplierCommand>("application/json")
                   .Produces<EditSuppliersResponse>(200, "application/json")
                   .ProducesValidationProblem(400)
                   );
    }

    public override async Task HandleAsync(EditSupplierCommand req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class EditSupplierCommandHandler  : IRequestHandler<EditSupplierCommand, EditSuppliersResponse>
{
    private readonly AppDbContext _context;

    public EditSupplierCommandHandler (AppDbContext context)
    {
        _context = context;
    }

    public async Task<EditSuppliersResponse> Handle(EditSupplierCommand req, CancellationToken ct)
    {
        var entity = await _context.Suppliers.FirstOrDefaultAsync(f => f.Id == req.Id, ct) ?? throw new Exception($"Fornecedor {req.Id} Não Encontrado!", new KeyNotFoundException());
        
        entity.TradingName = req.TradingName;

        await _context.SaveChangesAsync(ct);

        return new EditSuppliersResponse(entity.Id, entity.Code, entity.TradingName, entity.CNPJ);

    }
}

public record EditSuppliersResponse(int Id, string Code, string TradingName, string CNPJ);