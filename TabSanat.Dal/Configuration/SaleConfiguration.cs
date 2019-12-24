using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TabSanat.Model;

namespace TabSanat.Dal.Configuration
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.Property(x => x.Date).HasColumnType("date");
            builder.Property(x => x.TotalPrice).HasColumnType("decimal(10,2)");

            builder.HasOne(x => x.Student).WithMany(x => x.Sales).OnDelete(DeleteBehavior.SetNull);

        }
    }
}
