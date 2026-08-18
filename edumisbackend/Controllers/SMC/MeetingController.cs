using edumis.DataAccess.IRepositories;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using edumis.Common;
using edumis.Models;
using edumisbackend.Common;

namespace edumisbackend.Controllers.SMC;

[Route("smc/[controller]")]
[ApiController]
[Authorize]
public class MeetingController(IUnitOfWork unitOfWork, IConfiguration configuration, SmcFileUploadHelper uploadHelper) : ControllerBase
{
    #region Add Meeting
    [HttpPost("add")]       
    public async Task<IActionResult> AddMeeting([FromForm] MeetingRequestDTO meeting, IFormFile? file = null)
    {
        if (meeting == null)
            return Ok(ResponseModel<string>.Failure("Invalid Details"));

        if(meeting.MeetingDate < DateOnly.FromDateTime(DateTime.Today))
        {
            return Ok(ResponseModel<string>.Failure("Past date meetings are not allowed to be created.", StatusCodes.Status403Forbidden));
        }

        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

        var tokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var branchUserId = tokenParam != null ? edumis.Common.Utilities.DecryptString(tokenParam) : string.Empty;

        var branchLoginDetails = await unitOfWork.SMCUserRepo.GetFirstOrDefault(x => x.UserId == new Guid(branchUserId) && x.IsValid == true);
        if (branchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound));

        if (file != null) {
            var validationResult = await uploadHelper.ValidateAsync(file);
            if(!validationResult.IsValid)
                return Ok(ResponseModel<string>.Failure(validationResult.Error?? "File validation error"));
        }

        var allBranchMembers = await unitOfWork.SMCMemberRegistrationsRepo.GetAll(x=>
            x.BranchId == branchLoginDetails.BranchId && 
            x.ForSession == currentSessionData.ForSession &&
            x.IsActive == true);
                
        var meetingModel = new MeetingModel
        {
            ForSession = currentSessionData.ForSession,
            BranchId = !string.IsNullOrEmpty(branchLoginDetails.BranchId) ? branchLoginDetails.BranchId : string.Empty,
            MeetingDate = meeting.MeetingDate,
            MeetingTime = meeting.MeetingTime,
            Title = meeting.Title,
            Invitees = allBranchMembers?.Select(x=>x.MemberId.ToString()).ToArray(),
            Status = (int)SMCMeetingStatus.ACTIVE,
            CreatedBy = branchUserId,
            ModifiedBy = branchUserId
        };        

        await unitOfWork.MeetingRepo.Add(meetingModel);

        if (meeting.Agenda is { Count: > 0 }) {
            var counter = 0;
            var agendaList = meeting.Agenda.Select(agenda => new MeetingAgendaModel() {
                    MeetingId = meetingModel.MeetingId,
                    SerialNo = ++counter,
                    AgendaCode = agenda.AgendaCode,
                    OtherDetails = agenda.OtherDetails,
                    CreatedBy = branchUserId,
                    ModifiedBy = branchUserId
                }).ToList();

            await unitOfWork.MeetingAgendaRepo.AddRange(agendaList);
        }

        if (file != null) {
            var fileDetails = await uploadHelper.UploadFile(file, Constants.SMC_MEETINGS, currentSessionData.ForSession,
                branchLoginDetails.BranchId);
            if (fileDetails.ErrorMessage != null) 
                return Ok(ResponseModel<string>.Failure(fileDetails.ErrorMessage));
            
            var attachmentSaveObj = new MeetingAttachmentsModel() {
                MeetingId = meetingModel.MeetingId,
                Title = meeting.AttachmentTitle,
                FileName = fileDetails.FileName,
                FilePath = fileDetails.FilePath,
                Extension = fileDetails.FileExtension,                
                SerialNo = 1,
                CreatedBy = branchUserId,
                ModifiedBy = branchUserId
            };
            await unitOfWork.MeetingAttachmentsRepo.Add(attachmentSaveObj);
        }

        await unitOfWork.Save();

        var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(branchUserId);
        if (branchDetails != null) {
            SmcAppNotifier.SendNotificationSilently(branchLoginDetails.BranchId, "New meeting created",
                $"A new meeting has been scheduled for {meeting.MeetingDate.ToString("d MMMM, yyyy")} by {branchDetails.BranchName} ({branchDetails.BranchId}).");
        }

        return Ok(ResponseModel<string>.Success(meetingModel.MeetingId.ToString(), "Meeting Created.", StatusCodes.Status201Created));
       
    }
    #endregion

    #region Get Meeting APIs
    [HttpGet("allmeetings/{session?}")]
    public async Task<IActionResult> GetAllMeetings([FromRoute] string? session)
    {
        if (string.IsNullOrEmpty(session))
        {
            var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
            if (currentSessionData == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

            session = currentSessionData.ForSession;
        }
        
        var branchid = User.Claims.FirstOrDefault(x => x.Type == "BranchId")?.Value;
        if (branchid.IsNullOrBlank())
            return Ok(ResponseModel<string>.Unauthorized());

        var meetings = await unitOfWork.MeetingRepo.GetMeetings(branchid, session);
        if (meetings == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));

        return Ok(meetings.OrderByDescending(n => n.MeetingDate).ToList());
    }

    [HttpGet("active_meetings/{session?}")]
    public async Task<IActionResult> GetActiveMeetings([FromRoute] string? session)
    {
        if (string.IsNullOrEmpty(session))
        {
            var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
            if (currentSessionData == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

            session = currentSessionData.ForSession;
        }
        
        var branchid = User.Claims.FirstOrDefault(x => x.Type == "BranchId")?.Value;
        if (branchid.IsNullOrBlank())
            return Ok(ResponseModel<string>.Unauthorized());

        var meetings = await unitOfWork.MeetingRepo.GetMeetings(branchid, session, (int)SMCMeetingStatus.ACTIVE);
        if (meetings == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        return Ok(meetings.OrderByDescending(n => n.MeetingDate).ToList());
    }

    [HttpGet("cancelled_meetings/{session?}")]
    public async Task<IActionResult> GetCancelledMeetings([FromRoute] string? session)
    {
        if (string.IsNullOrEmpty(session))
        {
            var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
            if (currentSessionData == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

            session = currentSessionData.ForSession;
        }

        var branchid = User.Claims.FirstOrDefault(x => x.Type == "BranchId")?.Value;
        if (branchid.IsNullOrBlank())
            return Ok(ResponseModel<string>.Unauthorized());
        
        var meetings = await unitOfWork.MeetingRepo.GetMeetings(branchid, session, (int)SMCMeetingStatus.CANCELLED);
        if (meetings == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        return Ok(meetings.OrderByDescending(n => n.MeetingDate).ToList());
    }

    [HttpGet("concluded_meetings/{session?}")]
    public async Task<IActionResult> GetConcludedMeetings([FromRoute] string? session)
    {
        if (string.IsNullOrEmpty(session))
        {
            var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
            if (currentSessionData == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

            session = currentSessionData.ForSession;
        }

        var branchid = User.Claims.FirstOrDefault(x => x.Type == "BranchId")?.Value;
        if (branchid.IsNullOrBlank())
            return Ok(ResponseModel<string>.Unauthorized());
        
        var meetings = await unitOfWork.MeetingRepo.GetMeetings(branchid, session, (int)SMCMeetingStatus.CONCLUDED);
        if (meetings == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        return Ok(meetings.OrderByDescending(n => n.MeetingDate).ToList());
    }

    [HttpGet("meetingdetails/{meetingid}")]
    public async Task<IActionResult> GetMeetingById([FromRoute] string meetingid)
    {
        var meetingDetails = await unitOfWork.MeetingRepo.GetMeetingDetails(new Guid(meetingid));
        if (meetingDetails == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        if (meetingDetails.MeetingAttachments != null && meetingDetails.MeetingAttachments.Count > 0)
        {            
            //string fileUploadPath = singleFileUpload.GetUploadPath(Constants.SMC_MEETINGS, meetingDetails.ForSession, BranchLoginDetails?.BranchId ?? string.Empty);

            foreach (var attachment in meetingDetails.MeetingAttachments)            
                attachment.FileURL = Path.Combine(configuration["UploadPath"]??"uploads", attachment?.FileURL ?? string.Empty).Replace("\\", "/");            
        }

        return Ok(meetingDetails);
    }
    #endregion

    #region Update APIs
    [HttpPost("deactivate/{meetingid}")]        
    public async Task<IActionResult> DeactivateMeeting([FromRoute] string meetingid)
    {
        if (string.IsNullOrEmpty(meetingid))
            return Ok(ResponseModel<string>.Failure("Invalid Details"));


        var meeting = await unitOfWork.MeetingRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(meetingid));
        if (meeting == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        if(meeting.MeetingDate < DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("Back date meetings can not be cancelled.", StatusCodes.Status403Forbidden));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        await unitOfWork.MeetingRepo.Deactivate(meetingid, BranchUserId);           

        var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(BranchUserId);
        if (branchDetails != null) {
            SmcAppNotifier.SendNotificationSilently(branchDetails.BranchId, "Meeting cancelled",
                $"Meeting scheduled for {meeting.MeetingDate.ToString("d MMMM, yyyy")} has been cancelled by {branchDetails.BranchName} ({branchDetails.BranchId}).");
        }
        return Ok(ResponseModel<string>.Success(meetingid, "Meeting Deactivated Successfully." ));
    }
        
    [HttpPost("update")]       
    public async Task<IActionResult> UpdateMeeting([FromBody] MeetingUpdateRequestDTO meetingData)
    {
        if (meetingData == null)
            return Ok(ResponseModel<string>.Failure("Invalid Details"));


        if (meetingData.MeetingDate < DateOnly.FromDateTime(DateTime.Today))
        {
            return Ok(ResponseModel<string>.Failure("Past date scheduled meetings are not allowed to be updated.", StatusCodes.Status403Forbidden));
        }
                
        var meeting = await unitOfWork.MeetingRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(meetingData.MeetingId));
        if (meeting == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        if(meeting.Status != (int)SMCMeetingStatus.ACTIVE)
            return Ok(ResponseModel<string>.Failure("Only current active meetings are allowed to be updated.", StatusCodes.Status403Forbidden));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var returnval = await unitOfWork.MeetingRepo.Update(meetingData, BranchUserId);
        if (!returnval)
        {
            return Ok(ResponseModel<string>.Failure("Unable to update the meeting details!"));
        }

        return Ok(ResponseModel<string>.Success(meeting.MeetingId.ToString(), "Meeting Details Updated Successfully."));

    }
    #endregion

    #region Agenda related APIs
    [HttpPost("addagenda/{meetingid}")]
    public async Task<IActionResult> AddAgenda([FromRoute] string meetingid, [FromBody] AgendaRequestDTO requestDTO)
    {
        if (string.IsNullOrEmpty(meetingid))
            return Ok(ResponseModel<string>.Failure("Invalid Details"));


        var meeting = await unitOfWork.MeetingRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(meetingid));
        if (meeting == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        if(meeting.MeetingDate < DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("New agenda can not be added to past scheduled meeting.", StatusCodes.Status403Forbidden));

        if (meeting.Status != (int)SMCMeetingStatus.ACTIVE)
            return Ok(ResponseModel<string>.Failure("New agenda can only be added to currently active meeting.", StatusCodes.Status403Forbidden));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var AllAgendas = await unitOfWork.MeetingAgendaRepo.GetAll(x => x.MeetingId == new Guid(meetingid));

        var meetingAgendaModels = AllAgendas.ToList();
        var nextAgendaSrNo = 1;
        if (meetingAgendaModels.Count > 0)
        {
            nextAgendaSrNo = meetingAgendaModels.Max(x => x.SerialNo) + 1;
        }

        var agendaData = new MeetingAgendaModel()
        {
            MeetingId = new Guid(meetingid), 
            SerialNo = nextAgendaSrNo,
            AgendaCode = requestDTO.AgendaCode,
            OtherDetails = requestDTO.OtherDetails,
            CreatedBy = BranchUserId,
            ModifiedBy = BranchUserId
        };

        await unitOfWork.MeetingAgendaRepo.Add(agendaData);
        await unitOfWork.Save();
        
        var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(BranchUserId);
        if (branchDetails != null) {
            SmcAppNotifier.SendNotificationSilently(branchDetails.BranchId, "Agenda added",
                $"A new agenda has been added to the meeting scheduled for {meeting.MeetingDate.ToString("d MMMM, yyyy")} by {branchDetails.BranchName} ({branchDetails.BranchId}).");
        }
        
        return Ok(ResponseModel<string>.Success(meetingid, "Meeting agenda added.", StatusCodes.Status201Created));
    }

    [HttpPost("remove_agenda/{meetingid}/{serialno}")]
    public async Task<IActionResult> RemoveAgenda([FromRoute] string meetingid, [FromRoute] int serialno)
    {
        if (string.IsNullOrEmpty(meetingid))
            return Ok(ResponseModel<string>.Failure("Invalid Details"));


        var meeting = await unitOfWork.MeetingRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(meetingid));
        if (meeting == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        if (meeting.MeetingDate < DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("Agenda can not be removed from past scheduled meeting.", StatusCodes.Status403Forbidden));

        if (meeting.Status != (int)SMCMeetingStatus.ACTIVE)
            return Ok(ResponseModel<string>.Failure("Agenda can only be removed from currently active meeting.", StatusCodes.Status403Forbidden));
               

        var meetingAgenda = await unitOfWork.MeetingAgendaRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(meetingid) && x.SerialNo == serialno);
        if (meetingAgenda == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));

        await unitOfWork.MeetingAgendaRepo.Remove(meetingAgenda);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success(meetingid, "Meeting Agenda Removed."));
    }
    #endregion

    #region Attachment related API
    [HttpPost("addattachment")]
    public async Task<IActionResult> AddAttachment([FromForm] AddMeetingAttachmentRequestDTO requestDTO, IFormFile? file) {
        
        if (file == null) 
            return Ok(ResponseModel<string>.Failure("Failed to add attachment file."));
        
        if (requestDTO == null)
            return Ok(ResponseModel<string>.Failure("Invalid Details"));

        
        var meeting = await unitOfWork.MeetingRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(requestDTO.MeetingId));
        if (meeting == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        if (meeting.Status == (int)SMCMeetingStatus.CANCELLED)
            return Ok(ResponseModel<string>.Failure("Meeting is already cancelled.", StatusCodes.Status403Forbidden));

        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var BranchLoginDetails = await unitOfWork.SMCUserRepo.GetFirstOrDefault(x => x.UserId == new Guid(BranchUserId) && x.IsValid == true);
        if (BranchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound));

        var validationResult = await uploadHelper.ValidateAsync(file);
        if(!validationResult.IsValid)
            return Ok(ResponseModel<string>.Failure(validationResult.Error?? "File validation error"));
        
        var fileDetails = await uploadHelper.UploadFile(file, Constants.SMC_MEETINGS, currentSessionData.ForSession,
            BranchLoginDetails.BranchId);
        if (fileDetails.ErrorMessage != null) {
            return Ok(ResponseModel<string>.Failure(fileDetails.ErrorMessage));
        }
        var allAttachments = ( await unitOfWork.MeetingAttachmentsRepo
                .GetAll(x => x.MeetingId == new Guid(requestDTO.MeetingId))
            ).ToList();
        
        var fileSrNo = 1;
        if (allAttachments.Count > 0)
            fileSrNo = allAttachments.Max(x => x.SerialNo) + 1;
        

        var AttachmentSaveObj = new MeetingAttachmentsModel() {
            MeetingId = new Guid(requestDTO.MeetingId),
            Title = requestDTO.AttachmentTitle,
            FileName = fileDetails.FileName,
            FilePath = fileDetails.FilePath,
            Extension = fileDetails.FileExtension,
            SerialNo = fileSrNo,
            CreatedBy = BranchUserId,
            ModifiedBy = BranchUserId
        };
        await unitOfWork.MeetingAttachmentsRepo.Add(AttachmentSaveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success(requestDTO.MeetingId, "Attachment file added.",StatusCodes.Status201Created));
    

    }

    [HttpPost("remove_attachment/{meetingid}/{serialno}")]
    public async Task<IActionResult> RemoveAttachment([FromRoute] string meetingid, [FromRoute] int serialno)
    {
        if (string.IsNullOrEmpty(meetingid))
            return Ok(ResponseModel<string>.Failure("Invalid Details"));


        var meeting = await unitOfWork.MeetingRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(meetingid));
        if (meeting == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));

        
        if (meeting.Status != (int)SMCMeetingStatus.ACTIVE)
            return Ok(ResponseModel<string>.Failure("Attachment can only be removed from currently active meeting.", StatusCodes.Status403Forbidden));

        var meetingAttachment = await unitOfWork.MeetingAttachmentsRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(meetingid) && x.SerialNo == serialno);
        if (meetingAttachment == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        var isRemoved = uploadHelper.RemoveFile(meetingAttachment.FilePath??"", meetingAttachment.FileName??"");
        if(isRemoved) {
            await unitOfWork.MeetingAttachmentsRepo.Remove(meetingAttachment);
            await unitOfWork.Save();
        }

        return Ok(ResponseModel<string>.Success(meetingid, "Attachment file removed."));
    }
    #endregion

    #region Post Meeting API
    [HttpPost("conclude")]
    public async Task<IActionResult> ConcludeMeeting([FromForm] ConcludeMeetingRequestDTO requestDTO, IFormFile? file = null)
    {
        if (requestDTO == null)
            return Ok(ResponseModel<string>.Failure("Invalid Details"));


        var meeting = await unitOfWork.MeetingRepo.GetFirstOrDefault(x => x.MeetingId == new Guid(requestDTO.MeetingId));
        if (meeting == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        if (meeting.Status != (int)SMCMeetingStatus.ACTIVE)
            return Ok(ResponseModel<string>.Failure("Only active meeting can be concluded.", StatusCodes.Status403Forbidden ));

        if (requestDTO.MeetingResolutions != null) {
            foreach (var resolution in requestDTO.MeetingResolutions) {
                if (resolution.AgendaSrNo == null || resolution.AgendaSrNo.Length <= 0) continue;
                var validAgendas = await unitOfWork.MeetingRepo.ValidateMeetingAgendaSerialNos(new Guid(requestDTO.MeetingId), resolution.AgendaSrNo);
                if (!validAgendas)
                    return Ok(ResponseModel<string>.Failure("Invalid agenda details.", StatusCodes.Status403Forbidden));
            }
        }

        var tokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var branchUserId = tokenParam != null ? Utilities.DecryptString(tokenParam) : string.Empty;

        if (file != null) {
            var validationResult = await uploadHelper.ValidateAsync(file);
            if(!validationResult.IsValid)
                return Ok(ResponseModel<string>.Failure(validationResult.Error?? "File validation error"));
        }

        var res = await unitOfWork.MeetingRepo.UpdatePostMeetingData(requestDTO, branchUserId);
        if (!res) 
            return Ok(ResponseModel<string>.Failure("Failed to concluded the meeting!"));
    
        #region File Upload

        if (file != null) {
            var fileDetails = await uploadHelper.UploadFile(file, Constants.SMC_MEETINGS, meeting.ForSession,
                meeting.BranchId);
            if (fileDetails.ErrorMessage != null)
                return Ok(ResponseModel<string>.Failure(fileDetails.ErrorMessage));
            
            var allAttachments = await unitOfWork.MeetingAttachmentsRepo.GetAll(x=>x.MeetingId == new Guid(requestDTO.MeetingId));
            var meetingAttachmentsModels = allAttachments.ToList();
            
            var fileSrNo = 1;
            if (meetingAttachmentsModels.Count > 0)
                fileSrNo = meetingAttachmentsModels.Max(x => x.SerialNo) + 1;
            
            var attachmentSaveObject = new MeetingAttachmentsModel() {
                MeetingId = new Guid(requestDTO.MeetingId),
                Title = requestDTO.AttachmentTitle,
                FileName = fileDetails.FileName,
                FilePath = fileDetails.FilePath,
                Extension = fileDetails.FileExtension,
                SerialNo = fileSrNo,
                CreatedBy = branchUserId,
                ModifiedBy = branchUserId
            };
            await unitOfWork.MeetingAttachmentsRepo.Add(attachmentSaveObject);  
        }

        
        #endregion

        #region Save Resolutions
        if (requestDTO.MeetingResolutions != null) {
            var allResolutions = requestDTO.MeetingResolutions
                .Select(resolution => new MeetingResolutionsModel() {
                    AgendaSrNo = resolution.AgendaSrNo,
                    MeetingId = new Guid(requestDTO.MeetingId),
                    Resolution = resolution.Resolution,
                    EstimatedCost = resolution.EstimatedCost,
                    IsClosed = false,
                    CreatedBy = branchUserId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = branchUserId,
                    ModifiedDate = DateTime.UtcNow
                }).ToList();
            await unitOfWork.MeetingResolutionsRepo.AddRange(allResolutions);                
        }
        #endregion

        await unitOfWork.Save();

        var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(branchUserId);
        if (branchDetails != null)
            SmcAppNotifier.SendNotificationSilently(branchDetails.BranchId, "Meeting Concluded",
                            $"Meeting scheduled for {meeting.MeetingDate.ToString("d MMMM, yyyy")} has been concluded by {branchDetails.BranchName} ({branchDetails.BranchId}).");
        
        return Ok(ResponseModel<string>.Success(requestDTO.MeetingId,"Meeting concluded."));
        
    }

    [HttpPost("close_resolution")]
    public async Task<IActionResult> CloseResolution([FromForm] CloseMeetingResolutionRequestDTO requestDTO, IFormFile? file = null) 
    {

        if (requestDTO == null)
            return Ok(ResponseModel<string>.Failure("Invalid Details"));


        var meetingResolution = await unitOfWork.MeetingResolutionsRepo.GetFirstOrDefault(x => x.ResolutionId == new Guid(requestDTO.ResolutionId));
        if (meetingResolution == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));


        if (meetingResolution.IsClosed ?? false)
            return Ok(ResponseModel<string>.Failure("Resolution already closed.", StatusCodes.Status403Forbidden));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        if (file != null) {
            var validationResult = await uploadHelper.ValidateAsync(file);
            if(!validationResult.IsValid)
                return Ok(ResponseModel<string>.Failure(validationResult.Error?? "File validation error"));
        }
        var meetingData = await unitOfWork.MeetingRepo.GetFirstOrDefault(x=>x.MeetingId == meetingResolution.MeetingId);            
        if(meetingData == null)
            return Ok(ResponseModel<string>.Failure("Invalid meeting", StatusCodes.Status500InternalServerError));
            
        var res = await unitOfWork.MeetingResolutionsRepo.CloseResolution(requestDTO, BranchUserId);
        if(!res)
            return Ok(ResponseModel<string>.Failure("Failed to close the resolution.", StatusCodes.Status500InternalServerError));
        
        if (file != null) {
            var fileDetails = await uploadHelper.UploadFile(file, Constants.SMC_MEETINGS, meetingData.ForSession,
                meetingData.BranchId);
            if (fileDetails.ErrorMessage != null) 
                return Ok(ResponseModel<string>.Failure(fileDetails.ErrorMessage));
            
            var allAttachments = (await unitOfWork.MeetingAttachmentsRepo
                .GetAll(x => x.MeetingId == meetingData.MeetingId)).ToList();
            
            var fileSrNo = 1;
            if (allAttachments.Count > 0)
                fileSrNo = allAttachments.Max(x => x.SerialNo) + 1;
            
            var AttachmentSaveObj = new MeetingAttachmentsModel() {
                MeetingId = meetingData.MeetingId,
                Title = requestDTO.AttachmentTitle,
                FileName = fileDetails.FileName,
                FilePath = fileDetails.FilePath,
                //ContentType = string.Empty,
                Extension = fileDetails.FileExtension,
                SerialNo = fileSrNo,
                CreatedBy = BranchUserId,
                ModifiedBy = BranchUserId
            };
            await unitOfWork.MeetingAttachmentsRepo.Add(AttachmentSaveObj);
        }
        
        await unitOfWork.Save();
        
        var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(BranchUserId);
        if (branchDetails != null) {
            SmcAppNotifier.SendNotificationSilently(branchDetails.BranchId, "Resolution closed",
                $"The resolution titled {meetingResolution.Resolution} has been closed by {branchDetails.BranchName} ({branchDetails.BranchId}).");
        }
        
        return Ok(ResponseModel<object?>.Success(null, "Resolution Closed."));
    

        
    }

    [HttpGet("resolutions/{fromdate}/{todate}/{closedonly?}")]
    public async Task<IActionResult> AllResolutions([FromRoute] DateOnly fromdate, [FromRoute] DateOnly todate, [FromRoute] bool? closedonly)
    {
        // var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;
        //
        // var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(BranchUserId);
        // if (branchDetails == null)
        //     return Ok(ResponseModel<string>
        //     {
        //         ReturnId = string.Empty,
        //         Message = "Branch Details Not Found.",
        //         Success = false,
        //         ReturnCode = StatusCodes.Status404NotFound.ToString()
        //     });
        //
        // if (!branchDetails.IsValid)
        //     return Ok(ResponseModel<string>
        //     {
        //         Success = false,
        //         Message = "Branch Deactivated!",
        //         ReturnCode = StatusCodes.Status403Forbidden.ToString()
        //     });

        var branchId = User.FindFirst("BranchId")?.Value??"";
        var returnData = await unitOfWork.MeetingResolutionsRepo.GetResolutionList(branchId, fromdate, todate);
        if (returnData == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));

        if (closedonly.HasValue)
            return closedonly == true ?
                    Ok(returnData.Where(x => x.IsClosed == true).OrderByDescending(x => x.CreatedDate)) :
                    Ok(returnData.Where(x => x.IsClosed is null || x.IsClosed == false).OrderByDescending(x => x.CreatedDate));

        else
            return Ok(returnData.OrderByDescending(x => x.CreatedDate));          
    }

    [HttpGet("resolutions/{resolutionid}")]
    public async Task<IActionResult> ResolutionDetails([FromRoute] string resolutionid)
    {
        if (string.IsNullOrEmpty(resolutionid))
            return Ok(ResponseModel<string>.Failure("Invalid Details"));

        var returnData = await unitOfWork.MeetingResolutionsRepo.ResolutionDetails(new Guid(resolutionid));
        if (returnData == null)
            return Ok(ResponseModel<string>.Failure("No Data Found!", StatusCodes.Status404NotFound));

        return Ok(returnData);
    }
    #endregion
}
