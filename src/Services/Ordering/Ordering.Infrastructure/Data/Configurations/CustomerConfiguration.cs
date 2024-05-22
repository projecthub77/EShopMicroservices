using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)  // queste configurazioni sono dovute all'uso di oggetti del DDD come i ValueObject 
        {                                                           // senza la quale il db non capirebbe come mappare alcuni tipi come as es. l'Id
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(
                    customerId => customerId.Value,
                    dbId => CustomerId.Of(dbId));

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(255);
            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
