using edumis.Models.Library.Books;
using edumis.Models.Library.Books.DTO;

namespace edumis.DataAccess.IRepositories.ILibrary.IBooks;

public interface IBookDetailsRepo : IRepository<BookDetailsModel>
{
    Task<int> Update(BookUpdateRequestDTO requestDTO, string UpdatedBy);
    Task<BookDetailsDTO?> GetBookDetails(Guid BookId);
    Task<IEnumerable<BookDetailsDTO>?> GetBooks(string BranchId);
}
