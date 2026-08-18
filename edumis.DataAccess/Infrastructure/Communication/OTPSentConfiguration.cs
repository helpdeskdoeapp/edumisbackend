using edumis.Models.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Communication;

internal sealed class OTPSentConfiguration : IEntityTypeConfiguration<OTPSentModel>
{
    public void Configure(EntityTypeBuilder<OTPSentModel> builder)
    {
        builder.HasKey(d => d.RowId);
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd(); 
    }
}
