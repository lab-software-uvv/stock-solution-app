using Microsoft.EntityFrameworkCore;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Features.Categories;

public record PutCategoriesRequest(int id, string name, string description) : IRequest<PutCategoriesResponse>;

public sealed class PutCategoriesEndpoint : Endpoint<PutCategoriesRequest, PutCategoriesResponse>
{
    private readonly ISender _mediator;

    public PutCategoriesEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Put("/categories/{id}");
        Description(c => c
                   .Accepts<PutCategoriesRequest>("application/json")
                   .Produces<PutCategoriesResponse>(200, "application/json")
                   .ProducesValidationProblem(400)
                   );
    }

    public override async Task HandleAsync(PutCategoriesRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class PutCategoriesHandler : IRequestHandler<PutCategoriesRequest, PutCategoriesResponse>
{
    private readonly AppDbContext _context;

    public PutCategoriesHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<PutCategoriesResponse> Handle(PutCategoriesRequest req, CancellationToken ct)
    {
        var entity = _context.Categories.FirstOrDefault(f => f.Id == req.id);

        if (entity == null)
            throw new Exception($"Categoria {req.id} Não Encontrada!", new KeyNotFoundException());

        entity.Name = req.name;
        entity.Description = req.description;

        _context.Categories.Update(entity);
        _context.SaveChanges();

        return Task.FromResult(new PutCategoriesResponse(entity.Id, entity.Name, entity.Description));

    }
}

public record PutCategoriesResponse(int Id, string Name, string Description);