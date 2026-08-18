using edumis.Models.Library.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Library.Books;

internal sealed class BookCatalogueConfiguration : IEntityTypeConfiguration<BookCatalogueModel>
{
    public void Configure(EntityTypeBuilder<BookCatalogueModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => new { x.BookId, x.AccessionNumber });

        builder.HasIndex(col => 
            col.AccessionNumber)
            .IsUnique();

        builder.HasOne(x => x.BookDetailsNavigation)
            .WithMany(x => x.BookCatalogueList)
            .HasForeignKey(x => x.BookId);
    }
}
