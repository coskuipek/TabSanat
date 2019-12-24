using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TabSanat.Model;

namespace TabSanat.Dal.Configuration
{
    public class ExtraConfiguration : IEntityTypeConfiguration<Extra>
    {
        public void Configure(EntityTypeBuilder<Extra> builder)
        {
            builder.Property(x => x.PriceToBuy).HasColumnType("decimal(10,2)");
            builder.Property(x => x.PriceToSell).HasColumnType("decimal(10,2)");
        }
    }
}
