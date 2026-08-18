using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class SMCFundTransactionsConfiguration : IEntityTypeConfiguration<SMCFundTransactionsModel>
{
    public void Configure(EntityTypeBuilder<SMCFundTransactionsModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x=> new { x.TransactionId });
        builder.HasOne(x=>x.MeetingNavigation)
            .WithMany()
            .HasForeignKey(x=>x.MeetingId);

        builder
            .HasMany(x => x.SMCTransactionAttachmentsList)
            .WithOne(x => x.SMCFundTransactionNavigation)
            .HasForeignKey(x=>x.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
            
    }
}
