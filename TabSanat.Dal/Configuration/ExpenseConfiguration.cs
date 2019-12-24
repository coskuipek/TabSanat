using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TabSanat.Model;

namespace TabSanat.Dal.Configuration
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.Property(x => x.Date).HasColumnType("date");
            builder.Property(x => x.PriceEach).HasColumnType("decimal(10,2)");
        }
    }
}
