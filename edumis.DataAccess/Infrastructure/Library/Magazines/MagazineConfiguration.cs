using edumis.Models.Library.Magazine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Library.Magazines;

internal sealed class MagazineConfiguration : IEntityTypeConfiguration<MagazineModel>
{
    public void Configure(EntityTypeBuilder<MagazineModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.MagazineId);
    }
}
