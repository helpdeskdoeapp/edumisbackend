using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Global;

[Table("tbgldesignationmenuitems")]
public class DesignationMenuItems : BaseEntity<int>
{     
    [Column(name: "designationid")]
    public int DesignationId { get; set; }
       
    [Column(name: "menuid")]
    public int MenuId { get; set; }

    [Column(name: "canview")]
    public bool? CanView { get; set; } = false;

    [Column(name: "cancreate")]
    public bool? CanCreate { get; set; } = false;

    [Column(name: "canedit")]
    public bool? CanEdit { get; set; } = false;

    [Column(name: "candelete")]
    public bool? CanDelete { get; set; } = false;    
}
