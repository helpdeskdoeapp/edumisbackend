using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class ExceptionLogsConfiguration : IEntityTypeConfiguration<ExceptionLogs>
{
    public void Configure(EntityTypeBuilder<ExceptionLogs> builder)
    {
        builder.Property(d => d.RowId)
       .HasColumnName("rowid")
       .ValueGeneratedOnAdd();

        builder.HasKey(d => d.RowId);
    }
}
