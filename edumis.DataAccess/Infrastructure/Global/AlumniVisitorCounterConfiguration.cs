using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class AlumniVisitorCounterConfiguration : IEntityTypeConfiguration<AlumniVisitorCounterModel>
{
    public void Configure(EntityTypeBuilder<AlumniVisitorCounterModel> builder)
    {
        builder.Property(d => d.RowId)
       .HasColumnName("rowid")
       .ValueGeneratedOnAdd();

        builder.HasKey(d => d.RowId);
    }
}
