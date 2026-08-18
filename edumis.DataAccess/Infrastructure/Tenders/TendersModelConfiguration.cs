using edumis.Models.Tenders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Tenders;

internal sealed class TendersModelConfiguration : IEntityTypeConfiguration<TendersModel>
{
    public void Configure(EntityTypeBuilder<TendersModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(d => d.RowId);
    }
}
