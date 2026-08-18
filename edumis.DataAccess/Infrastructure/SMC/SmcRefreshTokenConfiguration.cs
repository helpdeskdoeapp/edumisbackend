using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class SmcRefreshTokenConfiguration : IEntityTypeConfiguration<SmcRefreshTokenModel>
{
    public void Configure(EntityTypeBuilder<SmcRefreshTokenModel> builder)
    {
        builder.Property(d => d.RowId)
          .HasColumnName("rowid")
          .ValueGeneratedOnAdd();

        builder.HasKey(x => x.RowId);

        builder.Property(x => x.Token).HasMaxLength(250);
        builder.HasIndex(x => x.Token).IsUnique();
    }
}