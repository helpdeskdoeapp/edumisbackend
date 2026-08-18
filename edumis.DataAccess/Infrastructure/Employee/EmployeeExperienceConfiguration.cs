using edumis.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Employee;

internal sealed class EmployeeExperienceConfiguration : IEntityTypeConfiguration<EmployeeExperienceModel>
{
    public void Configure(EntityTypeBuilder<EmployeeExperienceModel> builder)
    {
        builder.Property(d => d.RowId)
       .HasColumnName("rowid")
       .ValueGeneratedOnAdd();

        builder.HasKey(x => new { x.EmployeeId, x.SerialNo });

        builder.HasOne(x => x.EmployeeNavigation)
           .WithMany(x => x.EmployeeExperiencesList)
           .HasForeignKey(x => x.EmployeeId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
