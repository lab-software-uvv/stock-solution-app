using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace StockSolution.Api.Features.Categories;

public record DeleteCategoriesRequest(int id) : IRequest<DeleteCategoriesResponse>;

public sealed class DeleteCategoriesEndpoint : Endpoint<DeleteCategoriesRequest, DeleteCategoriesResponse>
{
    private readonly ISender _mediator;

    public DeleteCategoriesEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/categories/{id}");
        Description(c => c
                    .Produces<DeleteCategoriesResponse>(200, "application/json")
                    .ProducesProblem(404)
                    );
    }

    public override async Task HandleAsync(DeleteCategoriesRequest req, CancellationToken ct) 
    {
        await SendAsync(await _mediator.Send(req));
    } 
}

public sealed class DeleteCategoriesHandler : IRequestHandler<DeleteCategoriesRequest, DeleteCategoriesResponse>
{
    private readonly AppDbContext _context;

    public DeleteCategoriesHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteCategoriesResponse> Handle(DeleteCategoriesRequest req, CancellationToken ct)
    {
        var entity = _context.Categories.FirstOrDefault(c => c.Id == req.id);
        if (entity == null)
        {
            throw new Exception($"Categoria {req.id} Não Encontrada!", new KeyNotFoundException());
        }
        else
        {   
            _context.Categories.Remove(entity);
            _context.SaveChanges();
        }

        return new DeleteCategoriesResponse(entity!.Id, entity.Name, entity.Description);
    }
}

public record DeleteCategoriesResponse(int Id, string Name, string Description);