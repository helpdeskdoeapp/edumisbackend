using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Masters;
using edumis.Models.Masters.DTO;
using Microsoft.AspNetCore.Mvc;

namespace edumisbackend.Controllers.Masters;

[Route("api/[controller]")]
[ApiController]
public class PostsController(IUnitOfWork UnitOfWork, IMapper Mapper) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] PostsDTO PostDetails)
    {
        if (PostDetails == null) return BadRequest();

        if (await UnitOfWork.PostsMaster.Exists(x => x.PostCode == PostDetails.PostCode))
            return BadRequest(new ResponseModel()
            {
                ReturnId = string.Empty,
                Message = "Post code " + PostDetails.PostCode + " already exists.",
                Success = false,
                ReturnCode = StatusCodes.Status400BadRequest.ToString()
            });

        var DataToBeSaved = Mapper.Map<PostsModel>(PostDetails);
        if (DataToBeSaved != null)
        {
            DataToBeSaved.CreatedBy = PostDetails.LoggedInUserId;
            DataToBeSaved.ModifiedBy = PostDetails.LoggedInUserId;

            await UnitOfWork.PostsMaster.Add(DataToBeSaved);
            await UnitOfWork.Save();
            return CreatedAtAction("GetPostCodeDetails", new { postcode = PostDetails.PostCode }, DataToBeSaved);
        }
        return BadRequest(new ResponseModel()
        {
            ReturnId = string.Empty,
            Message = "Invalid/Bad Request Details",
            Success = false,
            ReturnCode = StatusCodes.Status400BadRequest.ToString()
        });
    }

    [HttpPost("update")]
    public async Task<ActionResult> Update([FromBody] PostsDTO PostDetails)
    {
        if (string.IsNullOrEmpty(PostDetails.PostCode)) return BadRequest();

        if (!await UnitOfWork.PostsMaster.Exists(x => x.PostCode == PostDetails.PostCode)) return NotFound();

        bool returnval = await UnitOfWork.PostsMaster.Update(PostDetails);
        if (!returnval)
            return BadRequest(new ResponseModel()
            {
                ReturnId = PostDetails.PostCode,
                Message = "Failed To Update Post Details.",
                Success = false,
                ReturnCode = StatusCodes.Status400BadRequest.ToString()
            });

        return Ok(new ResponseModel()
        {
            ReturnId = PostDetails.PostCode,
            Message = "Post Details Updated Successfully.",
            Success = true,
            ReturnCode = StatusCodes.Status200OK.ToString()
        });
    }

    [HttpGet("postcodedetail/{postcode}")]
    public async Task<ActionResult<PostsDetailsDTO>> GetPostCodeDetails([FromRoute] string postcode)
    {
        var ReturnData = await UnitOfWork.PostsMaster.GetFirstOrDefault(x => x.PostCode == postcode);
        if (ReturnData == null)
            return NotFound();

        return Ok(new PostsDetailsDTO(
                    ReturnData.PostCode,
                    ReturnData.PostTitle,
                    ReturnData.IsGazetted,
                    ReturnData.OrderNo,
                    ReturnData.OrderDate,
                    ReturnData.IsValid
                ));
    }

    [HttpGet("postcodes")]
    public async Task<ActionResult<List<PostsDetailsDTO>>> GetPostCodes()
    {
        var ReturnData = await UnitOfWork.PostsMaster.GetAll();
        if (ReturnData == null)
            return NotFound();

        List<PostsDetailsDTO> AllPostCodes = new List<PostsDetailsDTO>();
        foreach (var item in ReturnData)
        {
            PostsDetailsDTO post = new PostsDetailsDTO(
                item.PostCode,
                item.PostTitle,
                item.IsGazetted,
                item.OrderNo,
                item.OrderDate,
                item.IsValid
            );
            AllPostCodes.Add(post);
        }

        return Ok(AllPostCodes);
    }
}
