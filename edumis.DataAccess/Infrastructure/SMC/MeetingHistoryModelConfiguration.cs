using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class MeetingHistoryModelConfiguration : IEntityTypeConfiguration<MeetingHistoryModel>
{
    public void Configure(EntityTypeBuilder<MeetingHistoryModel> builder)
    {
        builder.Property(d => d.RowID)
          .HasColumnName("rowid")
          .ValueGeneratedOnAdd();

        builder.HasKey(x => x.RowID);
    }
}
