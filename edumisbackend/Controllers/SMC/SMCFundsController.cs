using edumis.DataAccess.IRepositories;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using edumis.Models;
using edumisbackend.Common;

namespace edumisbackend.Controllers.SMC;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SMCFundsController(IUnitOfWork unitOfWork, SmcFileUploadHelper uploadHelper) : ControllerBase
{
    [HttpPost("addtransaction")]
    public async Task<IActionResult> AddTransaction([FromForm] SMCFundTransactionRequestDTO requestDTO, IFormFile? file = null)
    {
        if (requestDTO == null)
            return Ok(ResponseModel<string>.Failure("Invalid Details"));        

        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

        var tokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var branchUserId = tokenParam != null ? edumis.Common.Utilities.DecryptString(tokenParam) : string.Empty;

        var branchLoginDetails = await unitOfWork.SMCUserRepo.GetFirstOrDefault(x => x.UserId == new Guid(branchUserId) && x.IsValid == true);
        if (branchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound ));
        
        if (file != null) {
            var validationResult = await uploadHelper.ValidateAsync(file);
            if(!validationResult.IsValid)
                return Ok(ResponseModel<string>.Failure(validationResult.Error?? "File validation error"));
        }

        var resolutionDetails = await unitOfWork.MeetingResolutionsRepo.GetFirstOrDefault(x=>x.ResolutionId ==  new Guid(requestDTO.ResolutionId));
        if(resolutionDetails == null)
            return Ok(ResponseModel<string>.Failure("Unable to get the resolution!", StatusCodes.Status204NoContent));
        var meetingDetails = await unitOfWork.MeetingRepo.GetMeetingDetails( resolutionDetails.MeetingId );
        if(meetingDetails == null)
            return Ok(ResponseModel<string>.Failure("Meeting not found!", StatusCodes.Status204NoContent));

        var transactionModel = new SMCFundTransactionsModel() {
            MeetingId = meetingDetails.MeetingId,
            ResolutionId = new Guid(requestDTO.ResolutionId),
            ReferenceDocNo = requestDTO.ReferenceDocNo,
            Amount = requestDTO.Amount,
            Description = requestDTO.Description,
            TransactionDate = requestDTO.TransactionDate,
            TransactionMode = requestDTO.TransactionMode,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = branchUserId,
            ModifiedBy = branchUserId,
            ModifiedDate = DateTime.UtcNow
        };
                
        await unitOfWork.SMCFundTransactionsRepo.Add(transactionModel);
        await unitOfWork.MeetingResolutionsRepo.UpdateResolutionActualCost(new Guid(requestDTO.ResolutionId), requestDTO.Amount, branchUserId);
        await unitOfWork.SmcBudgetRepo.UpdateExpense(currentSessionData.ForSession, meetingDetails.BranchId, requestDTO.Amount, branchUserId);
        
        
        if (file != null) {
            var fileDetails = await uploadHelper.UploadFile(file, Constants.SMC_MEETINGS, currentSessionData.ForSession,
                branchLoginDetails.BranchId);
            if (fileDetails.ErrorMessage != null) {
                return Ok(ResponseModel<string>.Failure(fileDetails.ErrorMessage));
            }
            var allAttachments = await unitOfWork.SMCTransactionAttachmentsRepo.GetAll(x => x.TransactionId == transactionModel.TransactionId);
            var attachmentSaveObject = new SMCTransactionAttachmentsModel() {
                TransactionId = transactionModel.TransactionId,
                Title = requestDTO.AttachmentTitle ?? string.Empty,
                FileName = fileDetails.FileName,
                FilePath = fileDetails.FilePath,
                Extension = fileDetails.FileExtension,
                SerialNo = allAttachments?.Count() + 1 ?? 1,
                CreatedBy = branchUserId,
                ModifiedBy = branchUserId
            };
            await unitOfWork.SMCTransactionAttachmentsRepo.Add(attachmentSaveObject);
        }

        await unitOfWork.Save();

        var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(branchUserId);
        if (branchDetails != null) 
            SmcAppNotifier.SendNotificationSilently(branchDetails.BranchId, "Transaction added",
                $"A transaction of Rs.{requestDTO.Amount} has been added to the resolution titled {resolutionDetails.Resolution} by {branchDetails.BranchName} ({branchDetails.BranchId}).");
        
        return Ok(ResponseModel<string>.Success(transactionModel.TransactionId.ToString(),  "Fund Transaction Added.",StatusCodes.Status201Created ));
    }

    [HttpPost("deactivate_transaction/{transactionId}")]
    public async Task<IActionResult> DeactivateTransaction([FromRoute] string transactionId, [FromBody] TransactionDeactivateDto dto)
    {
        if (string.IsNullOrEmpty(transactionId))
            return Ok(ResponseModel<string>.Failure("Invalid Details"));     

        var transactionData = await unitOfWork.SMCFundTransactionsRepo.GetTransactionDetails(new Guid(transactionId));
        if (transactionData == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));
        
        var tokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var branchUserId = tokenParam is not null? edumis.Common.Utilities.DecryptString(tokenParam) : string.Empty;
        
        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

       
        var txn = await unitOfWork.SMCFundTransactionsRepo.GetFirstOrDefault(t=>t.TransactionId == new Guid(transactionId));
        if(txn?.MeetingId == null)
            return Ok(ResponseModel<string>.Failure("Invalid meeting Id!", StatusCodes.Status204NoContent));
        
        var meetingDetails = await unitOfWork.MeetingRepo.GetMeetingDetails( (Guid)txn.MeetingId);
        if(meetingDetails == null)
            return Ok(ResponseModel<string>.Failure("Meeting not found!", StatusCodes.Status204NoContent));
        
        await unitOfWork.SMCFundTransactionsRepo.Deactivate(new Guid(transactionId), dto.Remarks, branchUserId);
        // Amount needs to be subtracted, so negaive amount is being added to consumption
        await unitOfWork.SmcBudgetRepo.UpdateExpense(currentSessionData.ForSession, meetingDetails.BranchId, -transactionData.Amount, branchUserId);
        // For the particular resolution, deduct from actual expenditure
        await unitOfWork.MeetingResolutionsRepo.UpdateResolutionActualCost(transactionData.ResolutionId, -transactionData.Amount, branchUserId);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success("Transaction deactivated successfully."));

    }
    
    
    #region Attachment related API
    [HttpPost("addattachment")]
    public async Task<IActionResult> AddAttachment([FromForm] AddTransactionAttachmentRequestDTO requestDTO, IFormFile? file)
    {
        if (requestDTO == null)
            return Ok(ResponseModel<string>.Failure("Invalid Details"));

        var transAttachmentData = await unitOfWork.SMCTransactionAttachmentsRepo.GetFirstOrDefault(x => x.TransactionId == new Guid(requestDTO.TransactionId));
        if (transAttachmentData == null)
            return Ok(ResponseModel<string>.Failure("No Transaction Data Found!", StatusCodes.Status404NotFound));

        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

        var tokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var branchUserId = tokenParam != null ? edumis.Common.Utilities.DecryptString(tokenParam) : string.Empty;

        var branchLoginDetails = await unitOfWork.SMCUserRepo.GetFirstOrDefault(x => x.UserId == new Guid(branchUserId) && x.IsValid == true);
        if (branchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound));
        
        if (file == null) 
            return Ok(ResponseModel<string>.Failure("Failed to add attachment file."));
        
        var validationResult = await uploadHelper.ValidateAsync(file);
        if(!validationResult.IsValid)
            return Ok(ResponseModel<string>.Failure(validationResult.Error?? "File validation error"));
        
        var fileDetails = await uploadHelper.UploadFile(file, Constants.SMC_MEETINGS, currentSessionData.ForSession,
            branchLoginDetails.BranchId);
        if (fileDetails.ErrorMessage != null) 
            return Ok(ResponseModel<string>.Failure(fileDetails.ErrorMessage));
    
        var allAttachments = await unitOfWork.SMCTransactionAttachmentsRepo.GetAll(x => x.TransactionId == new Guid(requestDTO.TransactionId));
        var attachmentSaveObject = new SMCTransactionAttachmentsModel() {
            TransactionId = new Guid(requestDTO.TransactionId),
            Title = requestDTO.AttachmentTitle ?? string.Empty,
            FileName = fileDetails.FileName,
            FilePath = fileDetails.FilePath,
            Extension = fileDetails.FileExtension,
            SerialNo = allAttachments?.Count() + 1 ?? 1,
            CreatedBy = branchUserId,
            ModifiedBy = branchUserId
        };
        await unitOfWork.SMCTransactionAttachmentsRepo.Add(attachmentSaveObject);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success(requestDTO.TransactionId, "Attachment file added.", StatusCodes.Status201Created));

    }

    [HttpPost("remove_attachment/{transactionid}/{serialno}")]
    public async Task<IActionResult> RemoveAttachment([FromRoute] string transactionid, [FromRoute] int serialno)
    {
        if (string.IsNullOrEmpty(transactionid))
            return Ok(ResponseModel<string>.Failure("Invalid Details"));

        var transactionData = await unitOfWork.SMCTransactionAttachmentsRepo.GetFirstOrDefault(x => x.TransactionId == new Guid(transactionid));
        if (transactionData == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));
       
        var transAttachment = await unitOfWork.SMCTransactionAttachmentsRepo.GetFirstOrDefault(x => x.TransactionId == new Guid(transactionid) && x.SerialNo == serialno);
        if (transAttachment == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));

        var IsRemoved = uploadHelper.RemoveFile(transAttachment.FilePath, transAttachment.FileName);
        if (IsRemoved)
        {
            await unitOfWork.SMCTransactionAttachmentsRepo.Remove(transAttachment);
            await unitOfWork.Save();
        }

        return Ok(ResponseModel<string>.Success(transactionid, "Attachment file removed."));
    }
    #endregion

    #region Get Transaction Details API
    [HttpGet("transaction_details/{transactionid}")]
    public async Task<IActionResult> TransactionDetails([FromRoute] string transactionid)
    {
        if (string.IsNullOrEmpty(transactionid))
            return Ok(ResponseModel<string>.Failure("Invalid Details"));     

        var transactionData = await unitOfWork.SMCFundTransactionsRepo.GetTransactionDetails(new Guid(transactionid));
        if (transactionData == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));
        return Ok(transactionData);
    }

    [HttpGet("transactions/{fromdate}/{todate}")]
    public async Task<IActionResult> AllTransactions([FromRoute] DateOnly fromdate, [FromRoute] DateOnly todate)
    {
        var TokenBranchIdParam = User.FindFirst("BranchId")?.Value;
        //string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        //var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(BranchUserId);
        if (string.IsNullOrEmpty(TokenBranchIdParam))
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found.",StatusCodes.Status404NotFound));

        var branchDetails = await unitOfWork.BranchRepo.GetFirstOrDefault(x => x.BranchId == TokenBranchIdParam);
        if (branchDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found.",StatusCodes.Status404NotFound));


        if (!branchDetails.IsActive)
            return Ok(ResponseModel<string>.Failure("Branch Deactivated!", StatusCodes.Status403Forbidden ));

        var transactionsList = await unitOfWork.SMCFundTransactionsRepo.AllTransactions(TokenBranchIdParam, fromdate, todate);
        if (transactionsList == null || transactionsList.Count == 0)
            return Ok(ResponseModel<string>.Failure("No Data Found!",StatusCodes.Status404NotFound));
        return Ok(transactionsList);
    }
    #endregion

}
