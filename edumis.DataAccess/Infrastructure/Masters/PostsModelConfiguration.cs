using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters;

internal sealed class PostsModelConfiguration : IEntityTypeConfiguration<PostsModel>
{
    public void Configure(EntityTypeBuilder<PostsModel> builder)
    {
        builder.Property(d => d.RowId)
             .HasColumnName("rowid")
             .ValueGeneratedOnAdd();

        builder.HasKey(x => x.PostCode);
    }
}
