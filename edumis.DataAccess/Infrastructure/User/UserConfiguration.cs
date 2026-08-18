using edumis.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.User;

internal sealed class UserConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => x.UserId);
        //builder.HasOne(x => x.EmployeeNavigation)
        //    .WithOne(x => x.UserNavigation)
        //    .HasForeignKey<UserModel>(x => x.UniqueId)
        //    .OnDelete(DeleteBehavior.Cascade);
    }
}
