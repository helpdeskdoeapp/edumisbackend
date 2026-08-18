using edumis.DataAccess.IRepositories;
using edumis.Models.Library.Books.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Library.Books;

[Route("api/[controller]")]
[ApiController]
public class BookCatalogueController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpPost("update")]
    public async Task<IActionResult> UpdateCatalogueDetails([FromBody] BookCatalogueUpdateRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (!await unitOfWork.BookDetailsRepo.Exists(x=>x.BookId == requestDTO.BookId))
            return Ok(ResponseModel<string>.NoData("Book details not found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var rowsAffected = await unitOfWork.BookCatalogueRepo.UpdateDetails(requestDTO, BranchUserId);
        return Ok(ResponseModel<bool>.Success(rowsAffected > 0,
            rowsAffected > 0 ? "Book Catalogue Details Updated!" : "Failed to update the book catalogue details!",
            StatusCodes.Status200OK));
    }
}
