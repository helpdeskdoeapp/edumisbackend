using edumis.DataAccess.IRepositories.ILibrary.IBooks;
using edumis.Models.Library.Books;

namespace edumis.DataAccess.Repositories.Library.Books;

internal class BookProcurementTransactionRepo : Repository<ProcurementTransactionModel>, IBookProcurementTransactionRepo
{
    private readonly ApplicationDBContext dBContext;
    public BookProcurementTransactionRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
