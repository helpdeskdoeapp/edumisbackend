using edumis.Models.Employees;
using edumis.Models.Leave;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Leave;

internal sealed class LeaveRegisterConfiguration : IEntityTypeConfiguration<LeaveRegisterModel>
{
    public void Configure(EntityTypeBuilder<LeaveRegisterModel> builder)    {
        builder.HasKey(a => a.EmployeeId);
    }
}
