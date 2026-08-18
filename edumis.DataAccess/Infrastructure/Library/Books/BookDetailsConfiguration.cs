using edumis.Models.Library.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Library.Books;

internal sealed class BookDetailsConfiguration : IEntityTypeConfiguration<BookDetailsModel>
{
    public void Configure(EntityTypeBuilder<BookDetailsModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.BookId);
    }
}
