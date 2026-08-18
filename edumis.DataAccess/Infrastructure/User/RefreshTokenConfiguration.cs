using edumis.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.User;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenModel>
{
    public void Configure(EntityTypeBuilder<RefreshTokenModel> builder)
    {
        builder.Property(d => d.RowId)
          .HasColumnName("rowid")
          .ValueGeneratedOnAdd();

        builder.HasKey(x => x.RowId);

        builder.Property(x => x.Token).HasMaxLength(250);
        builder.HasIndex(x => x.Token).IsUnique();
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);       
    }
}