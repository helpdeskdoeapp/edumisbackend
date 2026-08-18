using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters;

internal sealed class ZonesModelConfiguration : IEntityTypeConfiguration<ZonesModel>
{
    public void Configure(EntityTypeBuilder<ZonesModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(d => d.RowId);

        builder.HasOne(x => x.DistrictNavigation)
            .WithMany(x => x.ZoneList)
            .HasForeignKey(x => x.DistrictId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

