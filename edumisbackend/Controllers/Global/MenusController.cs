using edumis.DataAccess.Repositories;
using edumis.Models.Global.DTO;
using edumis.Models.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using edumisbackend.Common;
using edumis.DataAccess.IRepositories;
using System.Security.Claims;
using edumis.Models.Library.Books.DTO;
using edumis.Models.Pagination;

namespace edumisbackend.Controllers.Global;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MenusController(IUnitOfWork UnitOfWork) : ControllerBase
{   
    [HttpPost("add")]    
    public async Task<IActionResult> AddMenu([FromBody] MenusDTO MenuData)
    {
        if (MenuData == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request!"));

        if (await UnitOfWork.MenusRepo.Exists(x => x.MenuTitle.ToUpper() == MenuData.MenuTitle.ToUpper()))
            return Ok(ResponseModel<string>.Failure("Menu with same title already exists."));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var LastMenuID = await UnitOfWork.MenusRepo.GetMaxMenuId();
        int newMenuID = LastMenuID.HasValue ? ((int)LastMenuID + 1) : 100;

        MenusModel menuObj = new MenusModel()
        {
            MenuId = newMenuID,
            MenuTitle = MenuData.MenuTitle,
            ParentMenuId = MenuData.ParentMenuId != null ? MenuData.ParentMenuId : null,
            Module = MenuData.Module != null ? MenuData.Module : null,
            IsValid = true,
            CreatedBy = BranchUserId,
            ModifiedBy = BranchUserId,
            Menuurl = MenuData.Menuurl
        };
        await UnitOfWork.MenusRepo.Add(menuObj);
        await UnitOfWork.Save();

        return Ok(ResponseModel<int>.Success(menuObj.MenuId,"Details saved successfully."));
    }

    [HttpGet("allmenus/{pageno}/{pagesize}")]    
    public async Task<IActionResult> GetAllMenus([FromRoute] int pageno, [FromRoute] int pagesize)
    {
        var allmenus = await UnitOfWork.MenusRepo.GetAllMenus();
        if (allmenus == null)
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        var sorted = allmenus.OrderBy(x => x.MenuId);

        var paginated = sorted
            .Skip((pageno - 1) * pagesize)
            .Take(pagesize)
            .ToList();

        var response = new PaginatedResponseDTO<MenuDetailDTO>
        {
            Items = paginated,
            PageNumber = pageno,
            PageSize = pagesize,
            TotalCount = sorted.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<MenuDetailDTO>>.Success(response, "Menus retrieved successfully."));
    }

    [HttpGet("getmenubyid/{menuid}")]    
    public async Task<IActionResult> GetMenuById([FromRoute] int menuid)
    {
        var allmenus = await UnitOfWork.MenusRepo.GetAllMenus();
        if (allmenus == null)
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        if (!allmenus.Any(x => x.MenuId == menuid))
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        return Ok(ResponseModel<MenuDetailDTO>.Success(allmenus.Where(x => x.MenuId == menuid).First(), "Menu details retrieved successfully"));       
    }

    [HttpPost("update")]   
    public async Task<ActionResult> Update([FromBody] MenusUpdateRequestDTO requestDTO)
    {
        if (requestDTO == null) return BadRequest(ResponseModel<string>.Failure("Invalid request!"));

        if (!await UnitOfWork.MenusRepo.Exists(x => x.MenuId == requestDTO.MenuId))
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        if (!requestDTO.IsValid)
        {
            if (await UnitOfWork.DesignationMenuItems.Exists(x => x.MenuId == requestDTO.MenuId))
                return Ok(ResponseModel<string>.Failure("Can not disable the menu item as the menu is already mapped to some designations. Please remove the designation - menu mapping first."));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var returnval = await UnitOfWork.MenusRepo.Update(requestDTO, BranchUserId);

        return returnval ? Ok(ResponseModel<string>.Success(string.Empty, "Menu Details Updated Successfully."))
                         :
                           Ok(ResponseModel<string>.Failure("Failed to update menu details."));
             
    }

    [HttpPost("update-status/{menuid}/{status}")]
    public async Task<ActionResult> UpdateStatus([FromRoute] int menuid, [FromRoute] bool status)
    {
        var menuDetails = await UnitOfWork.MenusRepo.GetFirstOrDefault(x => x.MenuId == menuid);
        if (menuDetails ==  null)
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        if (!status)
        {
            if (await UnitOfWork.DesignationMenuItems.Exists(x => x.MenuId == menuid))
                return Ok(ResponseModel<string>.Failure("Can not disable the menu item as the menu is already mapped to some designations. Please remove the designation - menu mapping first."));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        menuDetails.IsValid = status;
        menuDetails.ModifiedBy = BranchUserId;
        menuDetails.ModifiedDate = DateTime.UtcNow;

        await UnitOfWork.Save();

        return Ok(ResponseModel<string>.Success(string.Empty, status ? "Menu activated Successfully." : "Menu de-activated Successfully."));

    }
}
