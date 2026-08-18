using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using edumis.Models.Masters.DTO;

namespace edumis.Models.Leave;

public class LeaveApplicationRequestDto
{
    public required string EmployeeId { get; set; }
    public required int LeaveType { get; set; }
    public required int Days { get; set; }
    public required DateOnly FromDate { get; set; }
    public required DateOnly ToDate { get; set; }
    public required string Reason { get; set; }
    public required string LeaveStation { get; set; }
    public DateOnly? ChildDob { get; set; }
    public bool? NocNeeded { get; set; }
    public string? Address { get; set; }
}

public record AddLeaveDto(
    string EmployeeId,
    List<SingleAddLeaveDto> Leaves
);

public record SingleAddLeaveDto(
    LeaveType LeaveType,
    int Days,
    string? Comment
);

public record ProcessLeaveDto(
    string ApplicationId,
    string Action,
    string Comment);

public class LeaveApplicationsAtEmplDto
{
    public required string ApplicationId { get; set; }
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required string Designation { get; set; }
    public required LeaveType LeaveType { get; set; }
    public required int Days { get; set; }
    public required DateOnly FromDate { get; set; }
    public required DateOnly ToDate { get; set; }
    public required DateTime AppliedAt { get; set; }
    public required string LeaveStation { get; set; }
    public required LeaveStatus LeaveStatus { get; set; }
    public required string Reason { get; set; }
    public string? AddressDuringLeave { get; set; }
    public bool? LeaveWithNoc { get; set; }
    public DateOnly? ChildDob { get; set; }
    public BranchesNamesDTO ActionBranch { get; set; } = default;
    public DateTime? LastActionDate { get; set; }
};

public class LeaveApplicationsAtBranchDto
{
    public required string ApplicationId { get; set; }
    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }
    public required string Designation { get; set; }
    public required LeaveType LeaveType { get; set; }
    public required int Days { get; set; }
    public required DateOnly FromDate { get; set; }
    public required DateOnly ToDate { get; set; }
    public required DateTime AppliedAt { get; set; }
    public required string LeaveStation { get; set; }
    public required LeaveStatus LeaveStatus { get; set; }
    public required string Reason { get; set; }
    public required string? AddressDuringLeave { get; set; }
    public required bool? LeaveWithNoc { get; set; }
    public required DateOnly? ChildDob { get; set; }
    public required bool IsLastLevel { get; set; }
    public required string BranchRole { get; set; }
    public BranchesNamesDTO ActionBranch { get; set; } = default;
    public DateTime? LastActionDate { get; set; }
    
};

public class LeaveApplicationTrackDto {
    public required string ActorId;
    public required string ActorName;
    public string? Action = null;
    public string? Remark = null;
    public DateTime? Date = null;
}

public class LeaveBalanceDto {
    public required string EmployeeId { get; set; }
    public required float CasualLeave {get; set;}
    public required int SpecialCasualLeave {get; set;}
    public required int EarnedLeave {get; set;}
    public required int MaternityLeave {get; set;}
    public required int PaternityLeave {get; set;}
    public required int HalfPayLeave {get; set;}
    public required int ChildCareLeave {get; set;}
}

public class LeaveBalanceTrackDto {
    public required int LeaveType { get; set; }
    public required string ActionBy { get; set; }
    public required string ActionType { get; set; }
    public required DateTime ActionAt { get; set; }
    public required float Days { get; set; }
    public string? Comment { get; set; }
    public string? LeaveApplicationId { get; set; }
}