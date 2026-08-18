using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters;

internal sealed class AcademicClassesConfiguration : IEntityTypeConfiguration<AcademicClassesModel>
{
    public void Configure(EntityTypeBuilder<AcademicClassesModel> builder)
    {
        builder.Property(d => d.RowId)
          .HasColumnName("rowid")
          .ValueGeneratedOnAdd();                 

        builder.HasKey(x => x.RowId);
    }
}
