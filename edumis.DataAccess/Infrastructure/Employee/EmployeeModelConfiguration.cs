using edumis.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Employee;

internal sealed class EmployeeModelConfiguration : IEntityTypeConfiguration<EmployeeModel>
{
    public void Configure(EntityTypeBuilder<EmployeeModel> builder)
    {
        builder.HasKey(x => x.EmployeeId);
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();
    }
}
