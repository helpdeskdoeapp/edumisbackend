using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters;

internal sealed class DesignationConfiguration : IEntityTypeConfiguration<DesignationModel>
{
    public void Configure(EntityTypeBuilder<DesignationModel> builder)
    {
        builder.HasKey(x => x.RowId);
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();
    }
}
