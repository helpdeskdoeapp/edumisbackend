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

internal sealed class LeaveApplicationTrackConfiguration : IEntityTypeConfiguration<LeaveApplicationTrackModel>{
    public void Configure(EntityTypeBuilder<LeaveApplicationTrackModel> builder)    {
        builder.Property(a => a.RowId).ValueGeneratedOnAdd();
        builder.HasKey(a => a.RowId);
    }
}
