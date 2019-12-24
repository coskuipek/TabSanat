using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TabSanat.Model;

namespace TabSanat.Dal.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(x => x.BirthDate).HasColumnType("date");
            builder.Property(x => x.RegisterDate).HasColumnType("date");
            builder.Property(x => x.Balance).HasColumnType("decimal(10,2)");

        }
    }
}
