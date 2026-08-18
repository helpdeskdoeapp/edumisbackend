using edumis.DataAccess.IRepositories.ILibrary.IBooks;
using edumis.Models.Library.Books;
using edumis.Models.Library.Books.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Library.Books;

internal class BookCatalogueRepo : Repository<BookCatalogueModel>, IBookCatalogueRepo
{
    private readonly ApplicationDBContext dBContext;
    public BookCatalogueRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<int?> GetMaxAccessionNo(string branchId)
    {
        return await dBContext.BookCatalogue
            .Where(x => x.BookDetailsNavigation.BranchId == branchId)
            .OrderByDescending(x => x.AccessionSerialNo)
            .Select(x => x.AccessionSerialNo)
            .FirstOrDefaultAsync();
    }

    public async Task<int> UpdateDetails(BookCatalogueUpdateRequestDTO requestDTO, string UpdatedBy)
    {
        return await dBContext.BookCatalogue.Where(x => x.BookId == requestDTO.BookId && requestDTO.AccessionNumber.Contains(x.AccessionNumber))
            .ExecuteUpdateAsync(b => b               
                .SetProperty(prop => prop.Location, requestDTO.Location)
                .SetProperty(prop => prop.Shelf, requestDTO.Shelf)                              
                .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
                .SetProperty(prop => prop.ModifiedBy, UpdatedBy)
            );
    }
}
