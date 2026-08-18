using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Global.DTO;

#region Designation UserType Mapping
public record DesignationUserTypeMappingDTO(
    [Required] int DesignationId,
    [Required] int UserType,
    [Required] string UserId
    );

public record DesignationUserTypeMappingDetailsDTO(
   int DesignationId,
   string DesignationTitle,
   int UserType,
   string UserTypeDesc
   );
#endregion

#region Menu Items
public record MenusDTO(
    [Required][MaxLength(100)] string MenuTitle,
    int? ParentMenuId,
    int? Module,
    string? Menuurl    
 );

public record MenusUpdateRequestDTO(
    [Required] int MenuId,
    [Required][MaxLength(100)] string MenuTitle,
    int? ParentMenuId,
    int? Module,
    string? Menuurl,
    [Required] bool IsValid
 );

public class MenuDetailDTO
{ 
    public int MenuId { get; set; }
    public string MenuTitle { get; set; } = default!;
    public int? ParentMenuId { get; set; }
    public string? ParentMenuTitle { get; set; }
    public int? Module { get; set; }
    public string? ModuleTitle { get; set; }
    public string? Menuurl { get; set; }
    public bool IsValid { get; set; }
}
#endregion

#region Designation Menu Items
public record DesignationMenuItemsDTO(
    [Required] int DesignationId,
    [Required] int MenuId,
    [Required] bool CanView,
    [Required] bool CanCreate,
    [Required] bool CanEdit,
    [Required] bool CanDelete,
    [Required] string UserId
    );

public record DesignationMenuItemsDetailsDTO(
    int DesignationId,
    string DesignationTitle,
    int MenuId,
    string MenuTitle,
    bool CanView,
    bool CanCreate,
    bool CanEdit,
    bool CanDelete
    );

#endregion
