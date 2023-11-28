using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Features.Products;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record EditSaleRequest(int Id, DateTime sellingDate, decimal totalValue, PaymentMethod paymentMethod) : IRequest<EditSaleResponse>;

public sealed class EditSaleEndpoint  : Endpoint<EditSaleRequest, EditSaleResponse>
{
    private readonly ISender _mediator;

    public EditSaleEndpoint (ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Put("/sales/{id}");
        AllowAnonymous();
        Description(c => c
                   .Accepts<EditSaleRequest>("application/json")
                   .Produces<EditSaleResponse>(200, "application/json")
                   .ProducesValidationProblem(400));
    }

    public override async Task HandleAsync(EditSaleRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req, ct), cancellation: ct);
}

public sealed class EditSaleCommandHandler  : IRequestHandler<EditSaleRequest, EditSaleResponse>
{
    private readonly AppDbContext _context;

    public EditSaleCommandHandler (AppDbContext context)
    {
        _context = context;
    }

    public async Task<EditSaleResponse> Handle(EditSaleRequest req, CancellationToken ct)
    {
        var entity = await _context.Sales.FirstOrDefaultAsync(f => f.Id == req.Id, ct) ?? throw new Exception($"Venda {req.Id} Não Encontrada!", new KeyNotFoundException());

        if (entity.Status != SaleStatus.InElaboration)
            throw new Exception("Não é possível alterar uma venda que não está em elaboração.");
        
        entity.SellingDate = req.sellingDate;
        entity.TotalValue = req.totalValue;
        entity.PaymentMethod = req.paymentMethod;

        await _context.SaveChangesAsync(ct);

        return new EditSaleResponse(entity.Id, entity.SellingDate, entity.TotalValue, entity.PaymentMethod, entity.Status);

    }
}

public record EditSaleResponse(int id, DateTime sellingDate, decimal totalValue, PaymentMethod paymentMethod, SaleStatus status);