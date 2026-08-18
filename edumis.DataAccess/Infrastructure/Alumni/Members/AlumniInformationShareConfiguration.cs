using edumis.Models.Alumni.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Alumni.Members;

internal sealed class AlumniInformationShareConfiguration : IEntityTypeConfiguration<AlumniInformationShareModel>
{
    public void Configure(EntityTypeBuilder<AlumniInformationShareModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.AlumniID);

        builder.HasOne(x => x.AlumniDetailNavigation)
            .WithOne(x => x.AlumniInformationShareDetails)
            .HasForeignKey<AlumniInformationShareModel>(e => e.AlumniID)
            .HasPrincipalKey<AlumniDetailsModel>(e => e.AlumniId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
