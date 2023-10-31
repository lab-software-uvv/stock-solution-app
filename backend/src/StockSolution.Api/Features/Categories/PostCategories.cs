using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StockSolution.Api.Persistence.Entities;
using StockSolution.Api.Validators;

namespace StockSolution.Api.Features.Categories;

public record PostCategoriesRequest(string name, string description) : IRequest<PostCategoriesResponse>;

public sealed class PostCategoriesEndpoint : Endpoint<PostCategoriesRequest, PostCategoriesResponse>
{
    private readonly ISender _mediator;

    public PostCategoriesEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Post("/categories");
        Description(c => c
                    .Accepts<PostCategoriesRequest>("application/json")
                    .Produces<PostCategoriesResponse>(201, "application/json")
                    .ProducesValidationProblem(400)
                    );
    }

    public override async Task HandleAsync(PostCategoriesRequest req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class PostCategoriesHandler : IRequestHandler<PostCategoriesRequest, PostCategoriesResponse>
{
    private readonly AppDbContext _context;

    public PostCategoriesHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<PostCategoriesResponse> Handle(PostCategoriesRequest req, CancellationToken ct)
    {
        var entity = _context.Categories.Add(new Category(req.name, req.description)).Entity;
        _context.SaveChanges();


        return Task.FromResult(new PostCategoriesResponse(entity.Id, entity.Name, entity.Description));
    }
}

public record PostCategoriesResponse(int Id, string Name, string Description);