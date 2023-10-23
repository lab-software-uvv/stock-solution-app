using Bogus;
using StockSolution.Api.Persistence.Entities;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace StockSolution.Api.Persistence;

[RegisterTransient]
public class DbInitializer : IAsyncInitializer
{
    private readonly AppDbContext _context;

    public DbInitializer(AppDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        // We do that as a safe-net since integration tests are going to use the InMemoryDatabase
        if (_context.Database.IsNpgsql())
        {
            await _context.Database.MigrateAsync(cancellationToken);
            await _context.Database.EnsureCreatedAsync(cancellationToken);
        }

        var hasEmployeeRoles = await _context.EmployeesRoles.AnyAsync(cancellationToken);

        if (!hasEmployeeRoles)
        {
            var employeeRoles = DataFactory.SeedEmployeeRoles();
            _context.EmployeesRoles.AddRange(employeeRoles);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public static class DataFactory
{
    public static IEnumerable<EmployeeRole> SeedEmployeeRoles()
    {
        var employeeRoles = new List<EmployeeRole>
        {
            new EmployeeRole
            {
                Description = "Funcionário",
            },
            new EmployeeRole
            {
                Description = "Adminstrador",
            },
            new EmployeeRole
            {
                Description = "Gerente",
            }
        };

        return employeeRoles;
    }
}