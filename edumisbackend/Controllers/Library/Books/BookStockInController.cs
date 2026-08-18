using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models.Enums.Library;
using edumis.Models.Library.Books;
using edumis.Models.Library.Books.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Library.Books;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookStockInController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpPost("update")]
    public async Task<IActionResult> UpdateStockInDetails([FromBody] BookProcurementUpdateRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (!await unitOfWork.BookDetailsRepo.Exists(x => x.BookId == requestDTO.BookId))
            return Ok(ResponseModel<string>.NoData("Book details not found!"));

        var BookCatalogueDetails = await unitOfWork.BookCatalogueRepo.GetFirstOrDefault(x => x.BookId == requestDTO.BookId);
        if (BookCatalogueDetails == null)
            return Ok(ResponseModel<string>.NoData("Book catalogue details not found!"));      

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var BranchLoginDetails = await unitOfWork.Users.GetUserDetails(new Guid(BranchUserId));
        if (BranchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

        var bookTransactionSaveObj = mapper.Map<BookProcurementUpdateRequestDTO, ProcurementTransactionModel>(requestDTO);      
        bookTransactionSaveObj.TransactionId = Guid.NewGuid();
        bookTransactionSaveObj.CreatedBy = BranchUserId;
        bookTransactionSaveObj.ModifiedBy = BranchUserId;
        await unitOfWork.BookProcurementTransactionRepo.Add(bookTransactionSaveObj);

        var currentAccessionNo = await unitOfWork.BookCatalogueRepo.GetMaxAccessionNo(BranchLoginDetails.BranchId);
        int startAccessionNo = currentAccessionNo.HasValue ? (int)currentAccessionNo : 0;

        List<BookCatalogueModel> bookCatalogueList = new List<BookCatalogueModel>();
        for (long i = 0; i < bookTransactionSaveObj.Quantity; i++)
        {
            var bookCatalogueSaveObj = new BookCatalogueModel
            {
                BookId = requestDTO.BookId,
                AccessionSerialNo = ++startAccessionNo,
                AccessionNumber = $"{DateTime.Today.Year}-{startAccessionNo:D4}",              
                Condition = BookCatalogueDetails.Condition,              
                Location = BookCatalogueDetails.Location,
                Shelf = BookCatalogueDetails.Shelf,
                Status = (int)BookStatusEnum.Available,
                CreatedBy = BranchUserId,
                ModifiedBy = BranchUserId
            };
            bookCatalogueList.Add(bookCatalogueSaveObj);
        }

        await unitOfWork.BookCatalogueRepo.AddRange(bookCatalogueList);

        var bookDetails = await unitOfWork.BookDetailsRepo.GetFirstOrDefault(x => x.BookId == requestDTO.BookId);
        bookDetails.Qty = bookDetails.Qty + requestDTO.Quantity;
        bookDetails.AvailableQty = bookDetails.AvailableQty + requestDTO.Quantity;
        bookDetails.ModifiedBy = BranchUserId;
        bookDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details saved successfully!", StatusCodes.Status200OK));        
    }
}
