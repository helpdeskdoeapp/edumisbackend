using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class MeetingModelConfiguration : IEntityTypeConfiguration<MeetingModel>
{
    public void Configure(EntityTypeBuilder<MeetingModel> builder)
    {
        builder.Property(d => d.RowId)
         .HasColumnName("rowid")
         .ValueGeneratedOnAdd();

        builder.HasKey(x => x.MeetingId);
    }
}
