using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockSolution.Api.Common;
using StockSolution.Api.Persistence.Entities;
using System.Globalization;
using System.IO;
using System.Text;

namespace StockSolution.Api.Features.VendasReport;

public record GetVendasReportQuery(DateTime InitialSaleDate, DateTime? FinalSaleDate) : IRequest<byte[]>;

public sealed class GetVendasReportEndpoint : Endpoint<GetVendasReportQuery, byte[]>
{
    private readonly ISender _mediator;

    public GetVendasReportEndpoint(ISender mediator)
        => _mediator = mediator;

    public override void Configure()
    {
        Get("/reports/vendas-report");
        
    }

    public override async Task HandleAsync(GetVendasReportQuery req, CancellationToken ct)
    {
        var r = await _mediator.Send(req);
        await SendBytesAsync(r, "Sales.csv", "text/csv");
    }
}

public sealed class GetVendasReportQueryHandler : IRequestHandler<GetVendasReportQuery, byte[]>
{
    private readonly AppDbContext _context;

    public GetVendasReportQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> Handle(GetVendasReportQuery req, CancellationToken ct)
    {
        var query = _context.Sales.AsNoTracking()
            .Where(p => p.SellingDate >= req.InitialSaleDate);

        if (req.FinalSaleDate.HasValue)
        {
            query = query.Where(p => p.SellingDate <= req.FinalSaleDate);
        }

        var vendas = await query.ToListAsync(ct);
        

        return CsvHelper<Sale>.GerarCsv(vendas);
    }
}