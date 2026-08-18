using edumis.DataAccess.IRepositories.ILibrary.IBooks;
using edumis.Models.Library.Books;

namespace edumis.DataAccess.Repositories.Library.Books;

internal class BookReviewsRepo : Repository<BookReviewsModel> , IBookReviewsRepo
{
    private readonly ApplicationDBContext dBContext;
    public BookReviewsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
}
