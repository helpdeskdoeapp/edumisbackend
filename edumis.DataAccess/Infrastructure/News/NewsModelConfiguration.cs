using edumis.Models.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.News;

internal sealed class NewsModelConfiguration : IEntityTypeConfiguration<NewsModel>
{
    public void Configure(EntityTypeBuilder<NewsModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(d => d.RowId);
    }
}
