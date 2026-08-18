using edumis.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Employee;

internal sealed class EmployeeEducationConfiguration : IEntityTypeConfiguration<EducationModel>
{
    public void Configure(EntityTypeBuilder<EducationModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => new {x.EmployeeId, x.SerialNo});
        builder.HasOne(x => x.EmployeeNavigation)
            .WithMany(x => x.EmployeeEducationList)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
