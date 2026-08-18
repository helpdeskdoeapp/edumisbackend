using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class SMCAccountsModelConfiguration : IEntityTypeConfiguration<SMCAccountsModel>
{
    public void Configure(EntityTypeBuilder<SMCAccountsModel> builder)
    {
        builder.Property(d => d.RowId)
          .HasColumnName("rowid")
          .ValueGeneratedOnAdd();

        builder.HasKey(x => x.UserId);
    }
}
