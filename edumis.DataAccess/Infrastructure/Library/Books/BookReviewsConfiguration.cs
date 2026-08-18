using edumis.Models.Library.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Library.Books;

internal sealed class BookReviewsConfiguration : IEntityTypeConfiguration<BookReviewsModel>
{
    public void Configure(EntityTypeBuilder<BookReviewsModel> builder)
    {
        builder.Property(d => d.RowId)
             .HasColumnName("rowid")
             .ValueGeneratedOnAdd();

        builder.HasKey(x => x.RowId);

        builder.HasOne(x => x.BookDetailsNavigation)
            .WithMany(x => x.BookReviewsList)
            .HasForeignKey(x => x.BookId);            
    }
}
