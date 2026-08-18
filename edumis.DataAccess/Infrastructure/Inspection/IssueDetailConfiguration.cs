using edumis.Models.Inspection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Inspection;

internal sealed class IssueDetailConfiguration : IEntityTypeConfiguration<IssueDetailModel>
{
    public void Configure(EntityTypeBuilder<IssueDetailModel> builder)
    {
        builder.Property(d => d.RowId)
               .HasColumnName("rowid")
               .ValueGeneratedOnAdd();

        builder.HasKey(d => d.IssueId);
    }
}
