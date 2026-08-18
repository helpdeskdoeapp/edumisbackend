using edumis.Models.Library.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Library.Books;

internal sealed class ProcurementTransactionConfiguration : IEntityTypeConfiguration<ProcurementTransactionModel>
{
    public void Configure(EntityTypeBuilder<ProcurementTransactionModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => new { x.BookId, x.TransactionId });

        builder.HasOne(x => x.BookDetailsNavigation)
            .WithMany(x => x.BookProcurementTransactionList)
            .HasForeignKey(x => x.BookId);
    }
}
