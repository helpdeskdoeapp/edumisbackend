using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters;

internal sealed class InfrastructureConfiguration : IEntityTypeConfiguration<InfrastructureModel>
{
    public void Configure(EntityTypeBuilder<InfrastructureModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => x.BuildingId);       
    }
}
