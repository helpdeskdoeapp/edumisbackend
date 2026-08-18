using edumis.Models.Alumni.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Alumni.Members;

internal sealed class AlumniDetailsConfiguration : IEntityTypeConfiguration<AlumniDetailsModel>
{
    public void Configure(EntityTypeBuilder<AlumniDetailsModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")          
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.AlumniId);

        builder.HasIndex(e => e.EmailID)
          .IsUnique();
    }
}
