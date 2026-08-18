using System.Security.Claims;
using AutoMapper;
using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace edumisbackend.Controllers.SMC;

[Route("smc/budget")]
[ApiController]
[Authorize]
public class SmcBudgetController(IUnitOfWork unitOfWork, IMapper mapper): ControllerBase {
    
    [HttpPost("donation")]
    public async Task<IActionResult> AddDonation([FromBody] SmcBudgetNewAllocationDto req) {
        
        if (req.Amount < 0) 
            return Ok(ResponseModel<string>.Failure("Amount allocated cant be negative", StatusCodes.Status406NotAcceptable ));
            
        
        var session = req.Session;
        if (session == null) {
            var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
            if (currentSessionData == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent ));
            session = currentSessionData.ForSession;
        }
        
        var tokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var branchUserId = tokenParam != null ? Utilities.DecryptString(tokenParam) : string.Empty;
        
        var allocation = new SmcBudgetAllocationHistoryModel {
            Session = session,
            SchoolId = req.SchoolId,
            Amount = req.Amount,
            AllocationType = (int)SMC_BUDGET_ALLOCATION_TYPE.DONATION,
            AllocationDate = req.AllocationDate?? DateTime.UtcNow,
            DonorName = req.DonorName,
            DonorPan = req.DonorPan,
            DonorMobile = req.DonorMobile,
            DonorAddress = req.DonorAddress,
            Remarks = req.Remarks,
            CreatedBy = branchUserId
            
        };
        await unitOfWork.SmcBudgetHistoryRepo.Add(allocation);
        await unitOfWork.SmcBudgetRepo.AddOrUpdateAllocation(session, req.SchoolId, req.Amount, branchUserId);
        await unitOfWork.Save();

        return Ok(ResponseModel<string?>.Success(null, "Donation added to budget.", StatusCodes.Status201Created));
    }

    [HttpGet("get/{session?}")]
    public async Task<IActionResult> GetBudget([FromRoute] string? session) {
        var tokenBranchId = User.FindFirst("BranchId")?.Value ?? null; 
        
        if (tokenBranchId is null)
            return Ok(ResponseModel<string>.Failure("Unauthorized access", StatusCodes.Status401Unauthorized));

        if (session == null) {
            var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
            if (currentSessionData == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));
            session = currentSessionData.ForSession;
        }
        
        var allocation = await unitOfWork.SmcBudgetRepo.GetFirstOrDefault(x=>x.Session==session && x.SchoolId==tokenBranchId);
        if (allocation == null)
            return Ok(ResponseModel<string>.Failure("Budget allocation not found", StatusCodes.Status204NoContent));
        
        return Ok( new SmcBudgetAllocationDetailDto(
            allocation.Session,
            allocation.SchoolId,
            allocation.Allocation,
            allocation.Consumption )
        );
    }
    
    [HttpGet("track/{session?}")]
    public async Task<IActionResult> GetBudgetHistory([FromRoute] string? session) {
        var tokenBranchId = User.FindFirst("BranchId")?.Value ?? null; 
        
        if (tokenBranchId == null)
            return Ok(ResponseModel<string>.Failure("Unauthorized access", StatusCodes.Status401Unauthorized ));
        

        if (session == null) {
            var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
            if (currentSessionData == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent ));
            session = currentSessionData.ForSession;
        }
        
        var allocations = await unitOfWork.SmcBudgetHistoryRepo.GetAllocationHistory(session, tokenBranchId);
        var response = allocations.OrderByDescending(n => n.AllocationDate);
        return Ok(response);
        
    }
    
    [HttpPost("allocation")]
    [Authorize("BudgetBranch")]
    public async Task<IActionResult> BulkAllocation([FromBody] List<SmcBudgetNewAllocationDto> reqs) {
        
        // TODO Authenticate the branch
        
        if (reqs.Any(r => r.Amount < 0)) {
            return Ok(ResponseModel<string>.Failure("Allocation amount can't be negative", StatusCodes.Status406NotAcceptable ));
        }

        string? defaultSession = null;
        if (reqs.Any(r => r.Session == null)) {
            defaultSession = (await unitOfWork.AcademicSessions
                .GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true))?.ForSession;
            if (defaultSession == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent ));
        }
        
        var tokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var branchUserId = tokenParam != null ? Utilities.DecryptString(tokenParam) : string.Empty;
        
        List <SmcBudgetAllocationHistoryModel> allocations = [];
        allocations.AddRange(reqs.Select(req => new SmcBudgetAllocationHistoryModel {
            Session = req.Session ?? defaultSession!,
            SchoolId = req.SchoolId,
            Amount = req.Amount,
            AllocationType = req.AllocationType,
            AllocationDate = req.AllocationDate ?? DateTime.UtcNow,
            Remarks = req.Remarks,
            CreatedBy = branchUserId
        }));
        
        await unitOfWork.SmcBudgetHistoryRepo.AddRange(allocations);
        foreach (var allocation in allocations) {
            await unitOfWork.SmcBudgetRepo.AddOrUpdateAllocation(allocation.Session, allocation.SchoolId, allocation.Amount, branchUserId);
        }
        await unitOfWork.Save();
        
        return Ok(ResponseModel<string?>.Success(null, "Budget allocated.", StatusCodes.Status201Created ));
    }
}