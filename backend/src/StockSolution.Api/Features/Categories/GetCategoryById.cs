using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace StockSolution.Api.Features.Categories;

public record GetCategoryByIdQuery(int Id) : IRequest<GetCategoryByIdResponse>;

public sealed class GetCategoryByIdEndpoint : Endpoint<GetCategoryByIdQuery, GetCategoryByIdResponse>
{
    private readonly ISender _mediator;

    public GetCategoryByIdEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/categories/{id}");
        
    }

    public override async Task HandleAsync(GetCategoryByIdQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdResponse>
{
    private readonly AppDbContext _context;

    public GetCategoryByIdQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetCategoryByIdResponse> Handle(GetCategoryByIdQuery req, CancellationToken ct)
    {
        var entity = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(f => f.Id == req.Id, ct);

        return entity is null
            ? throw new Exception($"Categoria {req.Id} Não Encontrada!", new KeyNotFoundException())
            : new GetCategoryByIdResponse(entity!.Id, entity.Name, entity.Description);
    }
}

public record GetCategoryByIdResponse(int Id, string Name, string Description);