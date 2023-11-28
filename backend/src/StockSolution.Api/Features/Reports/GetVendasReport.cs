using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockSolution.Api.Common;
using StockSolution.Api.Persistence.Entities;
using System.IO;

namespace StockSolution.Api.Features.VendasReport;

public record GetVendasReportQuery(DateTime InitialSaleDate, DateTime? FinalSaleDate) : IRequest<FileContentResult>;

public sealed class GetVendasReportEndpoint : Endpoint<GetVendasReportQuery, FileContentResult>
{
    private readonly ISender _mediator;

    public GetVendasReportEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/reports/vendas-report");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetVendasReportQuery req, CancellationToken ct)
        => await SendAsync(await _mediator.Send(req));
}

public sealed class GetVendasReportQueryHandler : IRequestHandler<GetVendasReportQuery, FileContentResult>
{
    private readonly AppDbContext _context;

    public GetVendasReportQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FileContentResult> Handle(GetVendasReportQuery req, CancellationToken ct)
    {
        //TODO: Alterar para dados de Venda
        var query = _context.Products.AsNoTracking();

        query = query.Where(p => p.AquisitionDate >= req.InitialSaleDate);

        if (req.FinalSaleDate.HasValue)
        {
            query = query.Where(p => p.AquisitionDate <= req.FinalSaleDate);
        }

        var vendas = await query.ToListAsync(ct);

        var csvWriter = new CsvWriter<Product>();
        var memoryStream = csvWriter.WriteRecords(vendas);

        return new FileContentResult(memoryStream.ToArray(), "text/csv")
        {
            FileDownloadName = "Vendas.csv"
        };
    }
}