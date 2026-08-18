using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class CodesModelConfiguration : IEntityTypeConfiguration<CodesModel>
{
    public void Configure(EntityTypeBuilder<CodesModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.Code);

        builder
            .HasMany(x => x.CodeValuesList)
            .WithOne(x => x.CodesNavigation)
            .HasForeignKey(x => x.Code)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
