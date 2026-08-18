using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class MemberDeviceTokensConfiguration : IEntityTypeConfiguration<MemberDeviceTokensModel>
{
    public void Configure(EntityTypeBuilder<MemberDeviceTokensModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.MemberId, d.SerialNo });
    }
}
