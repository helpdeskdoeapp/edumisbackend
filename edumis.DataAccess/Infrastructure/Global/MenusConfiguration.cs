using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class MenusConfiguration : IEntityTypeConfiguration<MenusModel>
{
    public void Configure(EntityTypeBuilder<MenusModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.MenuId });
    }
}   
