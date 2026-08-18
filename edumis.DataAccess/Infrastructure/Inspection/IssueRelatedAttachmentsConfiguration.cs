using edumis.Models.Inspection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Inspection;

internal sealed class IssueRelatedAttachmentsConfiguration : IEntityTypeConfiguration<IssueRelatedAttachmentsModel>
{
    public void Configure(EntityTypeBuilder<IssueRelatedAttachmentsModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => new {x.IssueId, x.SerialNo});
        builder.HasOne(x => x.InspectionIssueNavigation)
            .WithMany()
            .HasForeignKey(x => x.IssueId)
            .HasPrincipalKey(x => x.IssueId);
    }
}
