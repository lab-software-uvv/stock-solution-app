using Microsoft.EntityFrameworkCore;
namespace StockSolution.Api.Features.Suppliers;

public record DeleteSuppliers(int Id) : IRequest;

public sealed class DeleteSuppliersEndpoint : Endpoint<DeleteSuppliers>
{
    private readonly ISender _mediator;

    public DeleteSuppliersEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/suppliers/{id}");
        AllowAnonymous();
        Description(c => c
                    .Produces(200)
                    .ProducesProblem(404)
                    );
    }

    public override async Task HandleAsync(DeleteSuppliers req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteSuppliersCommandHandler : IRequestHandler<DeleteSuppliers>
{
    private readonly AppDbContext _context;

    public DeleteSuppliersCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSuppliers req, CancellationToken ct)
    {
        await _context.Suppliers.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);

        return;
    }
}
