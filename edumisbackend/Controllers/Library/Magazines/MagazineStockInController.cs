using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models.Library.Magazine;
using edumis.Models.Library.Magazine.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Library.Magazines;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MagazineStockInController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpPost("update")]
    public async Task<IActionResult> UpdateStockInDetails(MagazineProcurementUpdateRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var magazineDetails = await unitOfWork.MagazineRepo.GetFirstOrDefault(x => x.MagazineId == requestDTO.MagazineId);

        if (magazineDetails == null)
            return Ok(ResponseModel<string>.NoData("Magazine details not found!"));
        
        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        magazineDetails.TotalQty = magazineDetails.TotalQty + requestDTO.Quantity;
        magazineDetails.ModifiedBy = BranchUserId;
        magazineDetails.ModifiedDate = DateTime.UtcNow;

        var TransactionSaveObj = mapper.Map<MagazineProcurementUpdateRequestDTO, MagazineProcurementTransactionModel>(requestDTO);
        TransactionSaveObj.TransactionId = Guid.NewGuid();
        TransactionSaveObj.CreatedBy = BranchUserId;
        TransactionSaveObj.ModifiedBy = BranchUserId;
        await unitOfWork.MagazineProcurementTransactionRepo.Add(TransactionSaveObj);
        
        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details updated successfully!", StatusCodes.Status200OK));       
    }
}
