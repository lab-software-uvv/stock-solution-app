﻿using Microsoft.EntityFrameworkCore;
namespace StockSolution.Api.Features.Categories;

public record DeleteCategories(int Id) : IRequest;

public sealed class DeleteCategoriesEndpoint : Endpoint<DeleteCategories>
{
    private readonly ISender _mediator;

    public DeleteCategoriesEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Delete("/categories/{id}");
        
        Description(c => c
                    .Produces(200)
                    .ProducesProblem(404)
                    );
    }

    public override async Task HandleAsync(DeleteCategories req, CancellationToken ct) 
    {
        await _mediator.Send(req);
        await SendOkAsync();
    } 
}

public sealed class DeleteCategoriesCommandHandler : IRequestHandler<DeleteCategories>
{
    private readonly AppDbContext _context;

    public DeleteCategoriesCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCategories req, CancellationToken ct)
    {
        var produtosComCategoria = await _context.Products.Where(p => p.CategoryId == req.Id).ToListAsync(ct);
        if (produtosComCategoria.Any())
        {
            produtosComCategoria.ForEach(p => p.CategoryId = null);
            await _context.SaveChangesAsync(ct);
        }


        await _context.Categories.Where(c => c.Id == req.Id).ExecuteDeleteAsync(ct);

        return;
    }
}
