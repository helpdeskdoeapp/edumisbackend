using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class SMCTransactionAttachmentsConfiguration : IEntityTypeConfiguration<SMCTransactionAttachmentsModel>
{
    public void Configure(EntityTypeBuilder<SMCTransactionAttachmentsModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.TransactionId, d.SerialNo });
        builder.HasOne(x => x.SMCFundTransactionNavigation)
            .WithMany(a=>a.SMCTransactionAttachmentsList)
            .HasForeignKey(x => x.TransactionId);
    }
}
