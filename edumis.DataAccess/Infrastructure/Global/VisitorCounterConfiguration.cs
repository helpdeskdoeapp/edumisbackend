using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class VisitorCounterConfiguration : IEntityTypeConfiguration<VisitorCounterModel>
{
    public void Configure(EntityTypeBuilder<VisitorCounterModel> builder)
    {
        builder.Property(d => d.RowId)
        .HasColumnName("rowid")
        .ValueGeneratedOnAdd();

        builder.HasKey(d => d.RowId);
    }
}
