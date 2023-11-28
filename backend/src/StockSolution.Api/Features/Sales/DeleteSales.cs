using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record DeleteSale(int Id) : IRequest;

public sealed class DeleteSaleEndpoint : Endpoint<DeleteSale>
{
    private readonly ISender _mediator;

    public DeleteSaleEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/sales/{id}");
        
        Description(c => c
            .Produces(200)
            .ProducesProblem(404)
        );
    }

    public override async Task HandleAsync(DeleteSale req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteSaleCommandHandler : IRequestHandler<DeleteSale>
{
    private readonly AppDbContext _context;

    public DeleteSaleCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSale req, CancellationToken ct)
    {
        var sale = await _context.Sales.FirstOrDefaultAsync(c => c.Id == req.Id, ct) ?? throw new Exception($"Venda {req.Id} Não Encontrada!", new KeyNotFoundException());

        if (sale.Status != SaleStatusEnum.InElaboration)
            throw new Exception("Não é possível deletar uma venda que não esteja em elaboração");

        await _context.Sales.Where(s => s.Id == req.Id).ExecuteDeleteAsync(ct);
        
        return;
    }
}