using edumis.Models.Employees;
using edumis.Models.Leave;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edumis.DataAccess.Infrastructure.Leave;

internal sealed class LeaveApplicationConfiguration : IEntityTypeConfiguration<LeaveApplicationModel>{
    public void Configure(EntityTypeBuilder<LeaveApplicationModel> builder)    {
        builder.Property(a => a.ApplicationId).HasDefaultValueSql("nextval('leave_application_number')");
        builder.HasKey(a => a.ApplicationId);
    }
}
