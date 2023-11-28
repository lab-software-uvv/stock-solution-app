using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Sales;

public record CreateSaleRequest(int userId, DateTime sellingDate, decimal totalValue, PaymentMethodEnum paymentMethod) : IRequest<CreateSaleResponse>;

public sealed class CreateSaleEndpoint : Endpoint<CreateSaleRequest, CreateSaleResponse>
{
    private readonly ISender _mediator;

    public CreateSaleEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/sales");
        AllowAnonymous();
        Description(c => c
            .Accepts<CreateSaleRequest>("application/json")
            .Produces<CreateSaleResponse>(201, "application/json")
            .ProducesValidationProblem(400)
        );
    }

    public override async Task HandleAsync(CreateSaleRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class CreateSaleCommandHandler : IRequestHandler<CreateSaleRequest, CreateSaleResponse>
{
    private readonly AppDbContext _context;

    public CreateSaleCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateSaleResponse> Handle(CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.userId);
        
        Sale sale = new(user.Id, request.sellingDate, request.totalValue, request.paymentMethod);
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateSaleResponse(sale.Id, sale.SellingDate, sale.TotalValue, sale.PaymentMethod, sale.Status);
    }
}

public record CreateSaleResponse(int id, DateTime sellingDate, decimal totalValue, PaymentMethodEnum paymentMethod, SaleStatusEnum status);