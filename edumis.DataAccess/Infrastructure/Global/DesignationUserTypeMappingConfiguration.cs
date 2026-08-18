using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class DesignationUserTypeMappingConfiguration : IEntityTypeConfiguration<DesignationUserTypeMapping>
{
    public void Configure(EntityTypeBuilder<DesignationUserTypeMapping> builder)
    {

        builder.Property(d => d.RowId)
        .HasColumnName("rowid")
        .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.DesignationId, d.UserType });
    }
}
