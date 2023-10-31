using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace StockSolution.Api.Features.Categories;

public record GetCategoryByIdQuery(int id) : IRequest<GetCategoryByIdResponse>;

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

public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdResponse>
{
    private readonly AppDbContext _context;

    public GetCategoryByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<GetCategoryByIdResponse> Handle(GetCategoryByIdQuery req, CancellationToken ct)
    {
        var entity = _context.Categories.FirstOrDefault(f => f.Id == req.id);

        if (entity != null)
            return Task.FromResult(new GetCategoryByIdResponse(entity.Id, entity.Name, entity.Description));
        else
            throw new Exception($"Categoria {req.id} Não Encontrada!", new KeyNotFoundException());

    }
}

public record GetCategoryByIdResponse(int id, string name, string description);