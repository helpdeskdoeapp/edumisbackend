using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Global;

[Table("tbglmenus")]
public class MenusModel : BaseEntity<int>
{   
    [Column(name: "menuid")]
    public int MenuId { get; set; }

    [Column(name: "menutitle", TypeName = "varchar(150)")]
    public string MenuTitle { get; set; } = default!;

    [Column(name: "parentmenuid")]
    public int? ParentMenuId { get; set; } = 0;

    [Column(name: "module")]
    public int? Module { get; set; } = 0;

    [Column(name: "isvalid")]
    public bool IsValid { get; set; }

    [Column(name: "menuurl", TypeName = "varchar(500)")]
    public string? Menuurl { get; set; } = default!;
}
