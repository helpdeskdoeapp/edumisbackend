using edumis.Models.Library.Newspaper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Library.Newspapers;

internal sealed class NewspaperConfiguration : IEntityTypeConfiguration<NewspaperModel>
{
    public void Configure(EntityTypeBuilder<NewspaperModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.NewspaperId);
    }
}
