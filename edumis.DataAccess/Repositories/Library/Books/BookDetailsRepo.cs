using edumis.DataAccess.IRepositories.ILibrary.IBooks;
using edumis.Models.Library.Books;
using edumis.Models.Library.Books.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Library.Books;

internal class BookDetailsRepo : Repository<BookDetailsModel>, IBookDetailsRepo
{
    private readonly ApplicationDBContext dBContext;
    public BookDetailsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }       

    public async Task<int> Update(BookUpdateRequestDTO requestDTO, string UpdatedBy)
    {
        var rowsAffected = await dBContext.BookDetails
             .Where(x => x.BookId == requestDTO.BookId)
             .ExecuteUpdateAsync(x => x
                 .SetProperty(prop => prop.Title, requestDTO.Title)
                 .SetProperty(prop => prop.SubTitle, requestDTO.SubTitle)
                 .SetProperty(prop => prop.Publisher, requestDTO.Publisher)
                 .SetProperty(prop => prop.BookLevel, requestDTO.BookLevel)
                 .SetProperty(prop => prop.BookType, requestDTO.BookType)
                 .SetProperty(prop => prop.VolumeNumber, requestDTO.VolumeNumber)
                 .SetProperty(prop => prop.AuthorFirstName, requestDTO.AuthorFirstName)
                 .SetProperty(prop => prop.AuthorMiddleName, requestDTO.AuthorMiddleName)
                 .SetProperty(prop => prop.AuthorLastName, requestDTO.AuthorLastName)
                 .SetProperty(prop => prop.Language, requestDTO.Language)
                 .SetProperty(prop => prop.Subject, requestDTO.Subject)
                 .SetProperty(prop => prop.Editor, requestDTO.Editor)
                 .SetProperty(prop => prop.ClassCode, requestDTO.ClassCode)
                 .SetProperty(prop => prop.Genre, requestDTO.Genre)
                 .SetProperty(prop => prop.Description, requestDTO.Description)
                 .SetProperty(prop => prop.DDCNo, requestDTO.DDCNo)
                 .SetProperty(prop => prop.SubdivisionNo, requestDTO.SubdivisionNo)
                 .SetProperty(prop => prop.Notes, requestDTO.Notes)
                 .SetProperty(prop => prop.Tags, requestDTO.Tags)
                 //.SetProperty(prop => prop.Rating, requestDTO.Rating)
                 .SetProperty(prop => prop.EBookUrl, requestDTO.EBookUrl)
                 .SetProperty(prop => prop.AudioUrl, requestDTO.AudioUrl)
                  //.SetProperty(prop => prop.VideoUrl, requestDTO.VideoUrl)
                  //.SetProperty(prop => prop.RelatedBooks, requestDTO.RelatedBooks)
                  //.SetProperty(prop => prop.Awards, requestDTO.Awards)
                  .SetProperty(prop => prop.ISBN, requestDTO.ISBN)
                  .SetProperty(prop => prop.CallNumber, requestDTO.CallNumber)
                 .SetProperty(prop => prop.NumberOfPages, requestDTO.NumberOfPages)
                 .SetProperty(prop => prop.Edition, requestDTO.Edition)
                 .SetProperty(prop => prop.PublicationYear, requestDTO.PublicationYear)
                 .SetProperty(prop => prop.ModifiedBy, UpdatedBy)
                 .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
              );             

        return rowsAffected;

    }

    public async Task<BookDetailsDTO?> GetBookDetails(Guid BookId)
    {
        var branchesLookup = await dBContext.Branches
            .AsNoTracking()
            .ToDictionaryAsync(b => b.BranchId, b => b.BranchName);

        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(c => c.CodeValue, c => c.CodeValDescription);

        var bookDetailsEntity = await dBContext.BookDetails
            .AsNoTracking()
            .Include(b => b.BookCatalogueList)
            .Include(b => b.BookProcurementTransactionList)
            .FirstOrDefaultAsync(b => b.BookId == BookId);

        if (bookDetailsEntity == null) return null;

        var dto = new BookDetailsDTO
        {
            BookId = bookDetailsEntity.BookId,
            BranchId = bookDetailsEntity.BranchId,
            BranchName = branchesLookup.GetValueOrDefault(bookDetailsEntity.BranchId, string.Empty),

            BookLevel = bookDetailsEntity.BookLevel,
            BookLevelDesc = codeValuesLookup.GetValueOrDefault(bookDetailsEntity.BookLevel, string.Empty),

            BookType = bookDetailsEntity.BookType,
            BookTypeDesc = codeValuesLookup.GetValueOrDefault(bookDetailsEntity.BookType, string.Empty),

            VolumeNumber = bookDetailsEntity.VolumeNumber,
            Title = bookDetailsEntity.Title,
            SubTitle = bookDetailsEntity.SubTitle,
            AuthorFirstName = bookDetailsEntity.AuthorFirstName,
            AuthorMiddleName = bookDetailsEntity.AuthorMiddleName,
            AuthorLastName = bookDetailsEntity.AuthorLastName,
            Publisher = bookDetailsEntity.Publisher,
            Editor = bookDetailsEntity.Editor,

            ClassCode = bookDetailsEntity.ClassCode,
            ClassCodeDesc = bookDetailsEntity.ClassCode.HasValue
                ? codeValuesLookup.GetValueOrDefault(bookDetailsEntity.ClassCode.Value, string.Empty)
                : string.Empty,

            Subject = bookDetailsEntity.Subject,
            SubjectDesc = bookDetailsEntity.Subject.HasValue
                ? codeValuesLookup.GetValueOrDefault(bookDetailsEntity.Subject.Value, string.Empty)
                : string.Empty,

            Language = bookDetailsEntity.Language,
            LanguageDesc = codeValuesLookup.GetValueOrDefault(bookDetailsEntity.Language, string.Empty),

            Description = bookDetailsEntity.Description,
            DDCNo = bookDetailsEntity.DDCNo,
            SubdivisionNo = bookDetailsEntity.SubdivisionNo,
            CoverImageUrl = bookDetailsEntity.CoverImageUrl,
            CoverImageExtenstion = bookDetailsEntity.CoverImageExtenstion,
            CoverImageContentType = bookDetailsEntity.CoverImageContentType,
            Notes = bookDetailsEntity.Notes,
            Tags = bookDetailsEntity.Tags,
            Rating = bookDetailsEntity.Rating,
            EBookUrl = bookDetailsEntity.EBookUrl,
            AudioUrl = bookDetailsEntity.AudioUrl,
            VideoUrl = bookDetailsEntity.VideoUrl,
            Awards = bookDetailsEntity.Awards,
            Qty = bookDetailsEntity.Qty,
            AvailableQty = bookDetailsEntity.AvailableQty,
            ISBN = bookDetailsEntity.ISBN,
            CallNumber = bookDetailsEntity.CallNumber,
            Edition = bookDetailsEntity.Edition,
            PublicationYear = bookDetailsEntity.PublicationYear,
            NumberOfPages = bookDetailsEntity.NumberOfPages,
            
            Genre = bookDetailsEntity.Genre?
                .ToDictionary(id => id, id => codeValuesLookup.GetValueOrDefault(id, string.Empty)),

            RelatedBooks = bookDetailsEntity.RelatedBooks != null
                ? await dBContext.BookDetails
                    .AsNoTracking()
                    .Where(rb => bookDetailsEntity.RelatedBooks.Contains(rb.BookId))
                    .Select(rb => new RelatedBookDetailsDTO
                    {
                        BookId = rb.BookId,
                        Title = rb.Title,
                        SubTitle = rb.SubTitle,
                        CoverImageUrl = rb.CoverImageUrl,
                        CoverImageExtenstion = rb.CoverImageExtenstion,
                        CoverImageContentType = rb.CoverImageContentType,
                        Rating = rb.Rating,
                        AuthorFirstName = rb.AuthorFirstName,
                        AuthorLastName = rb.AuthorLastName,
                        AuthorMiddleName = rb.AuthorMiddleName 
                    }).ToListAsync()
                : new List<RelatedBookDetailsDTO>(),

            CatalogueDetails = bookDetailsEntity.BookCatalogueList != null && bookDetailsEntity.BookCatalogueList.Count() > 0
                ? bookDetailsEntity.BookCatalogueList.Select(c => new BookCatalogueDetailsDTO                  
                {
                    BookId = c.BookId,
                    AccessionNumber = c.AccessionNumber,                  
                    Location = c.Location,
                    Shelf = c.Shelf,                   
                    Condition = c.Condition,
                    ConditionDesc = codeValuesLookup.GetValueOrDefault(c.Condition, string.Empty),
                    ConditionNotes = c.ConditionNotes,
                    DamageType = c.DamageType,    
                    DamageTypeDesc = codeValuesLookup.GetValueOrDefault(c.DamageType ?? 0, string.Empty),
                    Status = c.Status,
                    StatusDesc = codeValuesLookup.GetValueOrDefault(c.Status, string.Empty)
                }).ToList()
                : null,

            BookProcurementTransactions = bookDetailsEntity.BookProcurementTransactionList?
                .Select(p => new BookProcurementTransactionDetailsDTO
                {
                    TransactionId = p.TransactionId,
                    ProcurementSource = p.ProcurementSource,
                    ProcurementSourceDesc = codeValuesLookup.GetValueOrDefault(p.ProcurementSource, string.Empty),
                    ProcurementDate = p.ProcurementDate,
                    OtherProcurementSource = p.OtherProcurementSource,
                    BillNo = p.BillNo,
                    BillDate = p.BillDate,
                    BillAmount = p.BillAmount,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToList()
        };

        return dto;

    }

    public async Task<IEnumerable<BookDetailsDTO>?> GetBooks(string BranchId)
    {
        var branchesLookup = await dBContext.Branches
            .AsNoTracking()
            .ToDictionaryAsync(b => b.BranchId, b => b.BranchName);

        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(c => c.CodeValue, c => c.CodeValDescription);

       var bookDetailsEntity = await dBContext.BookDetails
            .AsNoTracking()
            .Include(b => b.BookCatalogueList)          
            .Where(b => b.BranchId == BranchId).ToListAsync();

        if (bookDetailsEntity == null) return null;

        var returnData = bookDetailsEntity
            .Select(bookDetailsEntity => new BookDetailsDTO
            {
                BookId = bookDetailsEntity.BookId,
                BranchId = bookDetailsEntity.BranchId,
                BranchName = branchesLookup.GetValueOrDefault(bookDetailsEntity.BranchId, string.Empty),
                BookLevel = bookDetailsEntity.BookLevel,
                BookLevelDesc = codeValuesLookup.GetValueOrDefault(bookDetailsEntity.BookLevel, string.Empty),
                BookType = bookDetailsEntity.BookType,
                BookTypeDesc = codeValuesLookup.GetValueOrDefault(bookDetailsEntity.BookType, string.Empty),
                VolumeNumber = bookDetailsEntity.VolumeNumber,
                Title = bookDetailsEntity.Title,
                SubTitle = bookDetailsEntity.SubTitle,
                AuthorFirstName = bookDetailsEntity.AuthorFirstName,
                AuthorMiddleName = bookDetailsEntity.AuthorMiddleName,
                AuthorLastName = bookDetailsEntity.AuthorLastName,
                Publisher = bookDetailsEntity.Publisher,
                Editor = bookDetailsEntity.Editor,
                ClassCode = bookDetailsEntity.ClassCode,
                ClassCodeDesc = bookDetailsEntity.ClassCode.HasValue
                     ? codeValuesLookup.GetValueOrDefault(bookDetailsEntity.ClassCode.Value, string.Empty)
                     : string.Empty,
                Subject = bookDetailsEntity.Subject,
                SubjectDesc = bookDetailsEntity.Subject.HasValue
                    ? codeValuesLookup.GetValueOrDefault(bookDetailsEntity.Subject.Value, string.Empty)
                    : string.Empty,
                Language = bookDetailsEntity.Language,
                LanguageDesc = codeValuesLookup.GetValueOrDefault(bookDetailsEntity.Language, string.Empty),
                Description = bookDetailsEntity.Description,
                DDCNo = bookDetailsEntity.DDCNo,
                SubdivisionNo = bookDetailsEntity.SubdivisionNo,
                CoverImageUrl = bookDetailsEntity.CoverImageUrl,
                CoverImageExtenstion = bookDetailsEntity.CoverImageExtenstion,
                CoverImageContentType = bookDetailsEntity.CoverImageContentType,
                Notes = bookDetailsEntity.Notes,
                Tags = bookDetailsEntity.Tags,
                Rating = bookDetailsEntity.Rating,
                EBookUrl = bookDetailsEntity.EBookUrl,
                AudioUrl = bookDetailsEntity.AudioUrl,
                VideoUrl = bookDetailsEntity.VideoUrl,
                Awards = bookDetailsEntity.Awards,
                Qty = bookDetailsEntity.Qty,
                AvailableQty = bookDetailsEntity.AvailableQty,
                ISBN = bookDetailsEntity.ISBN,
                CallNumber = bookDetailsEntity.CallNumber,
                Edition = bookDetailsEntity.Edition,
                PublicationYear = bookDetailsEntity.PublicationYear,
                NumberOfPages = bookDetailsEntity.NumberOfPages,

                Genre = bookDetailsEntity.Genre?
                .ToDictionary(id => id, id => codeValuesLookup.GetValueOrDefault(id, string.Empty)),

                CatalogueDetails = bookDetailsEntity.BookCatalogueList != null && bookDetailsEntity.BookCatalogueList.Count() > 0
                ? bookDetailsEntity.BookCatalogueList.Select(c => new BookCatalogueDetailsDTO
                {
                    BookId = c.BookId,
                    AccessionNumber = c.AccessionNumber,
                    Location = c.Location,
                    Shelf = c.Shelf,
                    Condition = c.Condition,
                    ConditionDesc = codeValuesLookup.GetValueOrDefault(c.Condition, string.Empty),
                    ConditionNotes = c.ConditionNotes,
                    DamageType = c.DamageType,
                    DamageTypeDesc = codeValuesLookup.GetValueOrDefault(c.DamageType ?? 0, string.Empty),
                    Status = c.Status,
                    StatusDesc = codeValuesLookup.GetValueOrDefault(c.Status, string.Empty)
                }).ToList()
                : null,                            
            }).ToList();

        return returnData;
    }
}
