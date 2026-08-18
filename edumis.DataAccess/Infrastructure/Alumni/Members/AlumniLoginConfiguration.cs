using edumis.Models.Alumni.UserAccounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Alumni.Members;

internal sealed class AlumniLoginConfiguration : IEntityTypeConfiguration<AlumniLoginModel>
{
    public void Configure(EntityTypeBuilder<AlumniLoginModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(x => x.EmailID);
        builder.HasIndex(e => e.EmailID)
         .IsUnique();
    }
}
