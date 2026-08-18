using edumis.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Employee;

internal sealed class EmployeeAppointmentConfiguration : IEntityTypeConfiguration<AppointmentModel>
{
    public void Configure(EntityTypeBuilder<AppointmentModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => x.EmployeeId);
        builder.HasOne(x => x.EmployeeNavigation)
            .WithOne(x => x.EmployeeAppointmentNavigation)
            .HasForeignKey<AppointmentModel>(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
