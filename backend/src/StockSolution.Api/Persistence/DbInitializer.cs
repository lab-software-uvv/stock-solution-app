using StockSolution.Api.Persistence.Entities;
using Extensions.Hosting.AsyncInitialization;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using StockSolution.Api.Services;

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
        if (_context.Database.IsNpgsql())
        {
            await _context.Database.MigrateAsync(cancellationToken);
            await _context.Database.EnsureCreatedAsync(cancellationToken);
        }

        var hasRoles = await _context.Roles.AnyAsync(cancellationToken);
        if (!hasRoles)
        {
            var roles = DataFactory.CreateRoles();
            _context.Roles.AddRange(roles);
        }
        
        var hasUsers = await _context.Users.AnyAsync(cancellationToken);
        if (!hasUsers)
        {
            var user = DataFactory.CreateUser();
            _context.Users.Add(user);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public static class DataFactory
{
    public const string AdminLogin = "admin@gmail.com";
    public const string AdminPassword = "123456";
    
    public static User CreateUser()
    {
        var hasher = new PasswordHasher();
        
        return new User
        {
            Name = "John Doe",
            Cpf = "01234567890",
            BirthDate = new LocalDate(1990, 05, 17),
            Email = AdminLogin,
            RoleId = 1,
            PasswordHash = hasher.Hash(AdminPassword)
        };
    }


    public static IEnumerable<Role> CreateRoles()
    {
        var employeeRoles = new List<Role>
        {
            new Role
            {
                Id = 1,
                Name = "Admin",
            },
            new Role
            {
                Id = 2,
                Name = "Manager",
            },
            new Role
            {
                Id = 3,
                Name = "Employee",
            }
        };

        return employeeRoles;
    }
}