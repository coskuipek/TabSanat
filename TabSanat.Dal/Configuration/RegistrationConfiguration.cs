using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TabSanat.Model;

namespace TabSanat.Dal.Configuration
{
    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.Property(x => x.Price).HasColumnType("decimal(10,2)");
            builder.Property(x => x.PaymentLeft).HasColumnType("decimal(10,2)");

            builder.Property(x => x.LeaveDate).HasColumnType("date");
            builder.Property(x => x.RegisterDate).HasColumnType("date");
            builder.Property(x => x.StartToCourseDate).HasColumnType("date");
        }
    }
}
