using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class CodeValuesModelConfiguration : IEntityTypeConfiguration<CodeValuesModel>
{
    public void Configure(EntityTypeBuilder<CodeValuesModel> builder)
    {
        builder.Property(d => d.RowId)
          .HasColumnName("rowid")
          .ValueGeneratedOnAdd();

        builder.HasKey(x => new {x.Code, x.CodeValue});

        builder.HasOne(x => x.CodesNavigation)
           .WithMany(x => x.CodeValuesList)
           .HasForeignKey(x => x.Code)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
