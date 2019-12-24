using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TabSanat.Model;

namespace TabSanat.Dal.Configuration
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(10,2)");
            builder.Property(x => x.PaymentDate).HasColumnType("date");

            builder.HasOne(x => x.Student).WithMany(x => x.Payments).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
