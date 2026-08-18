namespace edumis.Models.Alumni.Members.DTO;

public class AlumniSearchRequestDTO
{        
    public int? District { get; set; }
    public int? Zone { get; set; }
    public string? BranchId { get; set; }
    public int? Gender { get; set; }
    public int? CurrentProfession { get; set; }
    public string? SortBy { get; set; } = "Name";
    public bool SortDescending { get; set; } = false;
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}
