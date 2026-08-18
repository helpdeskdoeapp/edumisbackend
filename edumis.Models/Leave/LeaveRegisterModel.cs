using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edumis.Models.Leave;

[Table("tble_leave_register")]
public class LeaveRegisterModel{
    [Column("employee_id")]
    public required string EmployeeId {get; set;}
   
    [Column("cl")]
    public float CasualLeave {get; set;}
   
    [Column("el")]
    public int EarnedLeave {get; set;}
    
    [Column("scl")]
    public int SpecialCasualLeave {get; set;}
    
    [Column("pl")]
    public int PaternityLeave {get; set;}
    
    [Column("ml")]
    public int MaternityLeave {get; set;}
    
    [Column("hpl")]
    public int HalfPayLeave {get; set;}
    
    [Column("ccl")]
    public int ChildCareLeave { get; set; }
    
    [Column("isactive")]
    public bool IsActive { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
}
