using edumis.Models.Library.Magazine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Library.Magazines;

internal sealed class MagazineProcurementTransactionConfiguration : IEntityTypeConfiguration<MagazineProcurementTransactionModel>
{
    public void Configure(EntityTypeBuilder<MagazineProcurementTransactionModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => new { x.MagazineId, x.TransactionId });

        builder.HasOne(x => x.MagazineNavigation)
            .WithMany(x => x.MagazineProcurementTransactionList)
            .HasForeignKey(x => x.MagazineId);
    }
}
