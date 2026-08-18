using edumis.Models.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Events;

internal sealed class EventsConfiguration : IEntityTypeConfiguration<EventsModel>
{
    public void Configure(EntityTypeBuilder<EventsModel> builder)
    {
        builder.Property(d => d.RowId)
            .HasColumnName("rowid")
            .ValueGeneratedOnAdd();

        builder.HasKey(d => d.RowId);
    }
}
