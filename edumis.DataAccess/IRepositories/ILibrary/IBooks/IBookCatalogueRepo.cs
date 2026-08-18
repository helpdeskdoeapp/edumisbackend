using edumis.Models.Library.Books;
using edumis.Models.Library.Books.DTO;

namespace edumis.DataAccess.IRepositories.ILibrary.IBooks;

public interface IBookCatalogueRepo : IRepository<BookCatalogueModel>
{
    Task<int?> GetMaxAccessionNo(string branchId);
    Task<int> UpdateDetails(BookCatalogueUpdateRequestDTO requestDTO, string UpdatedBy);
}
