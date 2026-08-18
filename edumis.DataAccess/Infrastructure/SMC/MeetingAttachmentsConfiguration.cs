using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class MeetingAttachmentsConfiguration : IEntityTypeConfiguration<MeetingAttachmentsModel>
{
    public void Configure(EntityTypeBuilder<MeetingAttachmentsModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.MeetingId, d.SerialNo });
        builder.HasOne(x => x.MeetingNavigation).WithMany().HasForeignKey(x => x.MeetingId);
    }
}
