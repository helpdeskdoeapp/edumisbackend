using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

internal sealed class MemberRegistrationsConfiguration : IEntityTypeConfiguration<MemberRegistrationsModel>
{
    public void Configure(EntityTypeBuilder<MemberRegistrationsModel> builder)
    {
        builder.Property(d => d.RowId)
           .HasColumnName("rowid")
           .ValueGeneratedOnAdd();

        builder.HasKey(d => new { d.ForSession, d.BranchId, d.UniqueId });
    }
}
