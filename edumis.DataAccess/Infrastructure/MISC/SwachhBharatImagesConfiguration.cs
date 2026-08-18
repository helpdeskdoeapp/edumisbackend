using edumis.Models.MISC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.MISC;

internal sealed class SwachhBharatImagesConfiguration : IEntityTypeConfiguration<SwachhBharatImagesModel>
{
    public void Configure(EntityTypeBuilder<SwachhBharatImagesModel> builder)
    {
        builder.HasKey(x => x.RowId);
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();
    }
}
