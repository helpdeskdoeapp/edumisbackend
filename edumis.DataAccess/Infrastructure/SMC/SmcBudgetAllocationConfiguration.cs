using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace edumis.DataAccess.Infrastructure.SMC;

public class SmcBudgetAllocationConfiguration: IEntityTypeConfiguration<SmcBudgetAllocationModel>
{
    public void Configure(EntityTypeBuilder<SmcBudgetAllocationModel> builder)
    {
        builder.Property(d => d.RowId)
            .ValueGeneratedOnAdd();

        builder.HasKey(x => new{x.Session,x.SchoolId});
    }
}
