using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters;

internal sealed class AcademicSubjectsConfiguration : IEntityTypeConfiguration<AcademicSubjectsModel>
{
    public void Configure(EntityTypeBuilder<AcademicSubjectsModel> builder)
    {
        builder.Property(d => d.RowId)
         .HasColumnName("rowid")
         .ValueGeneratedOnAdd();

        builder.HasKey(x => x.RowId);
    }
}
