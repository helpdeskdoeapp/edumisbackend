using edumis.Models.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Communication;

internal sealed class SMSTemplatesConfiguration : IEntityTypeConfiguration<SMSTemplatesModel>
{
    public void Configure(EntityTypeBuilder<SMSTemplatesModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.TemplateId);
    }
}
