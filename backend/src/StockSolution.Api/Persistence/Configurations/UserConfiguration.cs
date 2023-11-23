using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockSolution.Api.Persistence.Entities;

namespace StockSolution.Api.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasIndex(x => x.Cpf)
            .IsUnique();
    }
}