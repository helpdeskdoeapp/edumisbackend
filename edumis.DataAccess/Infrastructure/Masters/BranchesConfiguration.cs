using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters;

internal sealed class BranchesConfiguration : IEntityTypeConfiguration<BranchesModel>
{
    public void Configure(EntityTypeBuilder<BranchesModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => x.BranchId);

        //builder.HasOne(b => b.InfrastructureNavigation)
        //    .WithMany(i => i.ListBranches)
        //    .HasForeignKey(b => b.BuildingId)
        //    .HasPrincipalKey(i => i.BuildingId)
        //    .IsRequired(false);        
    }
}
