using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TabSanat.Model;

namespace TabSanat.Dal.Configuration
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {

            builder.Property(x => x.PriceEach).HasColumnType("decimal(10,2)");
            builder.HasKey(o => new { o.ExtraId, o.SaleId });
        }
    }
}
