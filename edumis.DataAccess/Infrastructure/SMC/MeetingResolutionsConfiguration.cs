using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class MeetingResolutionsConfiguration : IEntityTypeConfiguration<MeetingResolutionsModel>
{
    public void Configure(EntityTypeBuilder<MeetingResolutionsModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.ResolutionId });
        builder.HasOne(x=>x.MeetingNavigation).WithMany().HasForeignKey(x => x.MeetingId);
    }
}
