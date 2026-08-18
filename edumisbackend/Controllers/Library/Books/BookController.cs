using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Enums.Library;
using edumis.Models.Library.Books;
using edumis.Models.Library.Books.DTO;
using edumis.Models.Pagination;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Library.Books;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookController(IUnitOfWork unitOfWork, IMapper mapper, SingleFileUpload singleFileUpload) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddBook([FromForm] BookRequestDTO requestDTO, IFormFile? file = null)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));
       
        if (await unitOfWork.BookDetailsRepo.Exists(x => x.ISBN == requestDTO.ISBN))
            return BadRequest(ResponseModel<string>.Failure($"Book details already added against the ISBN [{requestDTO.ISBN}]!", StatusCodes.Status403Forbidden));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));
       
        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedImageExtensions;
            string[] allowedMimeTypes = Constants.AllowedImageMimeTypes;

            fileDetails = file != null
            ? await singleFileUpload.UploadFileInFolder(file, allowedExtensions, allowedMimeTypes, Constants.Library, BranchID)
            : null;

            if (fileDetails == null)
                return BadRequest(ResponseModel<string>.Failure("File Upload Failed!"));          
        }

        var bookDetailsSaveObj = mapper.Map<BookRequestDTO, BookDetailsModel>(requestDTO);
        bookDetailsSaveObj.BookId = Guid.NewGuid();
        bookDetailsSaveObj.BranchId = BranchID;
        bookDetailsSaveObj.Qty = requestDTO.BookProcurementDetails.Quantity;
        bookDetailsSaveObj.AvailableQty = requestDTO.BookProcurementDetails.Quantity;
        bookDetailsSaveObj.CreatedBy = BranchUserId;
        bookDetailsSaveObj.ModifiedBy = BranchUserId;

        if (fileDetails != null)
        {
            bookDetailsSaveObj.CoverImageUrl = fileDetails.FilePath;            
            bookDetailsSaveObj.CoverImageExtenstion = fileDetails.FileExtension;
            bookDetailsSaveObj.CoverImageContentType = fileDetails.FileMimeType;
        }
        await unitOfWork.BookDetailsRepo.Add(bookDetailsSaveObj);

        var bookTransactionSaveObj = mapper.Map<BookProcurementTransactionRequestDTO, ProcurementTransactionModel>(requestDTO.BookProcurementDetails);
        bookTransactionSaveObj.BookId = bookDetailsSaveObj.BookId;
        bookTransactionSaveObj.TransactionId = Guid.NewGuid();
        bookTransactionSaveObj.CreatedBy = BranchUserId;
        bookTransactionSaveObj.ModifiedBy = BranchUserId;
        await unitOfWork.BookProcurementTransactionRepo.Add(bookTransactionSaveObj);

        var currentAccessionNo = await unitOfWork.BookCatalogueRepo.GetMaxAccessionNo(BranchID);
        List<BookCatalogueModel> bookCatalogueList = new List<BookCatalogueModel>();
        int startAccessionNo = currentAccessionNo.HasValue ? (int)currentAccessionNo : 0;
       
        for (long i = 0; i < bookTransactionSaveObj.Quantity; i++)
        {
            var bookCatalogueSaveObj = new BookCatalogueModel
            {
                BookId = bookDetailsSaveObj.BookId,
                AccessionSerialNo = ++startAccessionNo,
                AccessionNumber = $"{DateTime.Today.Year}-{startAccessionNo:D4}",                 
                Condition = (int)BookConditionEnum.UnKnown,                
                Status = (int)BookStatusEnum.Available,
                CreatedBy = BranchUserId,
                ModifiedBy = BranchUserId
            };
            bookCatalogueList.Add(bookCatalogueSaveObj);
        }

        await unitOfWork.BookCatalogueRepo.AddRange(bookCatalogueList);

        await unitOfWork.Save();

        return Ok(ResponseModel<Guid>.Success(bookDetailsSaveObj.BookId, "Book Details Saved Successfully!", StatusCodes.Status201Created));        
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateBook([FromForm] BookUpdateRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));
      
        var rowsAffected = await unitOfWork.BookDetailsRepo.Update(requestDTO, BranchUserId);
        return Ok(ResponseModel<bool>.Success(rowsAffected > 0, 
            rowsAffected > 0 ? "Book Details Updated!" : "Failed to update the book details!", 
            StatusCodes.Status200OK));               
    }

    [HttpPost("updatecoverimage/{bookid}")]
    public async Task<IActionResult> UpdateCoverImage([FromRoute] string bookid, IFormFile file)
    {
        if (string.IsNullOrEmpty(bookid) || file == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

        var bookDetails = await unitOfWork.BookDetailsRepo.GetFirstOrDefault(x => x.BookId == new Guid(bookid));

        if (bookDetails == null)
            return NotFound(ResponseModel<string>.NoData("No book details found!"));

        string[] allowedExtensions = Constants.AllowedImageExtensions;
        string[] allowedMimeTypes = Constants.AllowedImageMimeTypes;
              
        var fileDetails = await singleFileUpload.UploadFileInFolder(file, allowedExtensions, allowedMimeTypes, Constants.Library, BranchID);
        if (fileDetails == null)
            return BadRequest(ResponseModel<string>.Failure("File Upload Failed!"));       

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        if (!string.IsNullOrEmpty(bookDetails.CoverImageUrl))
            singleFileUpload.RemoveFile(bookDetails.CoverImageUrl);

        bookDetails.CoverImageUrl = fileDetails.FilePath;
        bookDetails.CoverImageExtenstion = fileDetails.FileExtension;
        bookDetails.CoverImageContentType = fileDetails.FileMimeType;
        bookDetails.ModifiedBy = BranchUserId;
        bookDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Book Cover Image Updated Successfully!", StatusCodes.Status200OK));       
    }

    [HttpGet("details/{bookid}")]
    public async Task<IActionResult> GetBookDetails([FromRoute] string bookid)
    {
        if (string.IsNullOrEmpty(bookid))
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var bookDetails = await unitOfWork.BookDetailsRepo.GetBookDetails(new Guid(bookid));
        if(bookDetails == null)
             return NotFound(ResponseModel<string>.NoData("No book details found!"));
        return Ok(ResponseModel<BookDetailsDTO>.Success(bookDetails, "Book details retrieved successfully"));
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchBookRequestDTO requestDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if(string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

        var allBooks = await unitOfWork.BookDetailsRepo.GetBooks(BranchID);
        if (allBooks == null || !allBooks.Any())
            return NotFound(ResponseModel<string>.NoData("No record found!"));

        if (requestDTO.ClassCode.HasValue && requestDTO.ClassCode > 0)        
            allBooks = allBooks.Where(x => x.ClassCode == requestDTO.ClassCode);
        
        if(requestDTO.Subject.HasValue && requestDTO.Subject > 0)
            allBooks = allBooks.Where(x => x.Subject == requestDTO.Subject);

        if (requestDTO.Language.HasValue && requestDTO.Language > 0)
            allBooks = allBooks.Where(x => x.Language == requestDTO.Language);

        if (requestDTO.BookLevel.HasValue && requestDTO.BookLevel > 0)
            allBooks = allBooks.Where(x => x.BookLevel == requestDTO.BookLevel);

        if (requestDTO.BookType.HasValue && requestDTO.BookType > 0)
            allBooks = allBooks.Where(x => x.BookType == requestDTO.BookType);
        
        if (!string.IsNullOrEmpty(requestDTO.Title))
            allBooks = allBooks.Where(x => x.Title.Contains(requestDTO.Title, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(requestDTO.SubTitle))
            allBooks = allBooks.Where(x => x.SubTitle.Contains(requestDTO.SubTitle, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(requestDTO.Author))
            allBooks = allBooks.Where(x => (x.AuthorFirstName != null && x.AuthorFirstName.Contains(requestDTO.Author, StringComparison.OrdinalIgnoreCase)) ||
                                            (x.AuthorMiddleName != null && x.AuthorMiddleName.Contains(requestDTO.Author, StringComparison.OrdinalIgnoreCase)) ||
                                            (x.AuthorLastName != null && x.AuthorLastName.Contains(requestDTO.Author, StringComparison.OrdinalIgnoreCase)));

        if (!string.IsNullOrEmpty(requestDTO.Publisher))
            allBooks = allBooks.Where(x => x.Publisher != null && x.Publisher.Contains(requestDTO.Publisher, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(requestDTO.Editor))
            allBooks = allBooks.Where(x => x.Editor != null && x.Editor.Contains(requestDTO.Editor, StringComparison.OrdinalIgnoreCase));

        if (requestDTO.Rating.HasValue && requestDTO.Rating > 0)
            allBooks = allBooks.Where(x => x.Rating.HasValue && x.Rating >= requestDTO.Rating);

        if (requestDTO.Tags != null && requestDTO.Tags.Any())
            allBooks = allBooks.Where(x => x.Tags != null && requestDTO.Tags.Any(tag => x.Tags.Contains(tag, StringComparison.OrdinalIgnoreCase)));

        if (!allBooks.Any())
            return NotFound(ResponseModel<string>.NoData("No record found!"));

        var bookSearchResults = allBooks.Select(book => new BookSearchResultDTO
        {
            BookId = book.BookId,
            BranchId = book.BranchId,
            BranchName = book.BranchName,
            Title = book.Title,
            SubTitle = book.SubTitle,
            ISBN = book.ISBN ?? string.Empty,   
            BookLevel = book.BookLevel,
            BookLevelDesc = book.BookLevelDesc,
            BookType = book.BookType,
            BookTypeDesc = book.BookTypeDesc,
            VolumeNumber = book.VolumeNumber,            
            AuthorFirstName = book.AuthorFirstName,
            AuthorMiddleName = book.AuthorMiddleName,
            AuthorLastName = book.AuthorLastName,
            Publisher = book.Publisher,
            Editor = book.Editor,
            ClassCode = book.ClassCode,
            ClassCodeDesc = book.ClassCodeDesc,
            Subject = book.Subject,
            SubjectDesc = book.SubjectDesc,
            Language = book.Language,
            LanguageDesc = book.LanguageDesc,
            Description = book.Description,
            DDCNo = book.DDCNo,
            SubdivisionNo = book.SubdivisionNo,
            CoverImageUrl = book.CoverImageUrl,
            CoverImageExtenstion = book.CoverImageExtenstion,
            CoverImageContentType = book.CoverImageContentType,
            Rating = book.Rating,
            Qty = book.Qty,
            AvailableQty = book.AvailableQty
        }).ToList();

        var sorted = bookSearchResults.OrderBy(x => x.Title);

        var paginated = sorted
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToList();

        var response = new PaginatedResponseDTO<BookSearchResultDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
            TotalCount = bookSearchResults.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<BookSearchResultDTO>>.Success(response, "Book details retrieved successfully"));        
    }
}
