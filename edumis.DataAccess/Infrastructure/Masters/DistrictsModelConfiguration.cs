using edumis.Models.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.Masters
{
    internal sealed class DistrictsModelConfiguration : IEntityTypeConfiguration<DistrictsModel>
    {
        public void Configure(EntityTypeBuilder<DistrictsModel> builder)
        {
            builder.Property(d => d.RowId)
              .HasColumnName("rowid")
              .ValueGeneratedOnAdd();

            builder.HasKey(x => x.RowId);
        }
    }
}
