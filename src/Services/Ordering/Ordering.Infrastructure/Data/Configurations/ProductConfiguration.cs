using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)  // queste configurazioni sono dovute all'uso di oggetti del DDD come i ValueObject 
        {                                                           // senza la quale il db non capirebbe come mappare alcuni tipi come as es. l'Id
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasConversion(
                    productId => productId.Value,
                    dbId => ProductId.Of(dbId));

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();            
        }
    }
}
