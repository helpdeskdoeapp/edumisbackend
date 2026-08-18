using edumis.DataAccess.IRepositories.ILibrary.IMagazine;
using edumis.Models.Library.Magazine;

namespace edumis.DataAccess.Repositories.Library.Magazine;

internal class MagazineProcurementTransactionRepo : Repository<MagazineProcurementTransactionModel>, IMagazineProcurementTransactionRepo
{
    private readonly ApplicationDBContext dBContext;
    public MagazineProcurementTransactionRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
