using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

public class SmcBudgetAllocationHistoryConfiguration: IEntityTypeConfiguration<SmcBudgetAllocationHistoryModel>
{
    public void Configure(EntityTypeBuilder<SmcBudgetAllocationHistoryModel> builder)
    {
        builder.Property(d => d.RowId)
            .ValueGeneratedOnAdd();

        builder.HasKey(x => x.RowId);
    }
}
