using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class DesignationMenuItemsConfiguration : IEntityTypeConfiguration<DesignationMenuItems>
{
    public void Configure(EntityTypeBuilder<DesignationMenuItems> builder)
    {
        builder.Property(d => d.RowId)
        .HasColumnName("rowid")
        .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.DesignationId, d.MenuId });           
    }
}
