using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters;

internal sealed class SchoolDetailsConfiguration : IEntityTypeConfiguration<SchoolDetailsModel>
{
    public void Configure(EntityTypeBuilder<SchoolDetailsModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => x.BranchId);

        builder.HasOne(x => x.BranchNavigation)
            .WithOne()
            .HasPrincipalKey<BranchesModel>(x => x.BranchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
