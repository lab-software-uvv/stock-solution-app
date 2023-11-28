using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record FinishSaleRequest(int id) : IRequest;

public sealed class FinishSaleEndpoint  : Endpoint<FinishSaleRequest>
{
    private readonly ISender _mediator;

    public FinishSaleEndpoint (ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Put("/sales/{id}/finish");
        AllowAnonymous();
        Description(c => c
            .Accepts<FinishSaleRequest>()
            .Produces(200)
            .ProducesProblem(404)
        );
    }

    public override async Task HandleAsync(FinishSaleRequest req, CancellationToken ct)
    {        
        await _mediator.Send(req);
        await SendOkAsync();
    }
}

public sealed class FinishSaleCommand : IRequestHandler<FinishSaleRequest>
{
    private readonly AppDbContext _context;

    public FinishSaleCommand (AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(FinishSaleRequest req, CancellationToken ct)
    {
        var sale = await _context.Sales.Include(s => s.ComercialProducts)
            .Include(s => s.Products)
            .FirstOrDefaultAsync(f => f.Id == req.id, ct)
                   ?? throw new Exception($"Venda {req.id} Não Encontrada!", new KeyNotFoundException());

        if (sale.ComercialProducts.Count == 0 && sale.Products.Count == 0)
            throw new Exception("Para finalizar uma venda, é necessário informar pelo menos um produto ou produto comercial");
        
        if (sale.Status == SaleStatusEnum.Finished)
            throw new Exception("Venda já está finalizada.");
        
        sale.Status = SaleStatusEnum.Finished;

        await _context.SaveChangesAsync(ct);
    }
}