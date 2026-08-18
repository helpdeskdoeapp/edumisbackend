using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Global;

internal sealed class SessionInfoModelConfiguration : IEntityTypeConfiguration<SessionInfoModel>
{
    public void Configure(EntityTypeBuilder<SessionInfoModel> builder)
    {
        builder.Property(d => d.RowId)
               .HasColumnName("rowid")
               .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.ForSession });
    }
}
