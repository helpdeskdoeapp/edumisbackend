using edumis.Models.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Web;

internal sealed class MarqueeDetailsConfiguration : IEntityTypeConfiguration<MarqueeDetailsModels>
{
    public void Configure(EntityTypeBuilder<MarqueeDetailsModels> builder)
    {
        builder.Property(d => d.RowId)
       .HasColumnName("rowid")
       .ValueGeneratedOnAdd();

        builder.HasKey(d => d.RowId);
    }
}
