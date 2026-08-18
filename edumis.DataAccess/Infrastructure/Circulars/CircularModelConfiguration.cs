using edumis.Models.Circulars;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Circulars;

internal sealed class CircularModelConfiguration : IEntityTypeConfiguration<CircularModel>
{
    public void Configure(EntityTypeBuilder<CircularModel> builder)
    {
        builder.HasKey(x => x.RowId);
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();
    }
}
