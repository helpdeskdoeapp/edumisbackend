using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class MeetingAgendaConfiguration : IEntityTypeConfiguration<MeetingAgendaModel>
{
    public void Configure(EntityTypeBuilder<MeetingAgendaModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.MeetingId, d.SerialNo });
        builder.HasOne(x=>x.MeetingNavigation).WithMany().HasForeignKey(x => x.MeetingId);
    }
}
