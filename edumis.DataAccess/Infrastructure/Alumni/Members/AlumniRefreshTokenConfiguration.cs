using edumis.Models.Alumni.Members;
using edumis.Models.Alumni.UserAccounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Alumni.Members;

internal sealed class AlumniRefreshTokenConfiguration : IEntityTypeConfiguration<AlumniRefreshTokenModel>
{
    public void Configure(EntityTypeBuilder<AlumniRefreshTokenModel> builder)
    {
        builder.Property(d => d.RowId)
          .HasColumnName("rowid")
          .ValueGeneratedOnAdd();

        builder.HasKey(x => x.RowId);

        builder.Property(x => x.Token).HasMaxLength(250);
        builder.HasIndex(x => x.Token).IsUnique();
        builder.HasOne(x => x.User)
            .WithMany()
            .HasPrincipalKey(x => x.AlumniID)
            .HasForeignKey(x => x.UserId);      
    }
}
