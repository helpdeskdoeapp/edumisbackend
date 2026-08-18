using edumis.Models.Inspection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Inspection;

internal sealed class IssueRelatedCommentsConfiguration : IEntityTypeConfiguration<IssueRelatedCommentsModel>
{
    public void Configure(EntityTypeBuilder<IssueRelatedCommentsModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => new { x.IssueId, x.SerialNo });
        builder.HasOne(x => x.InspectionIssueNavigation)
            .WithMany()
            .HasForeignKey(x => x.IssueId)
            .HasPrincipalKey(x => x.IssueId);
    }
}
