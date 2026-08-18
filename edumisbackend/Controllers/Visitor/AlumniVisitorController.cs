using edumis.DataAccess.IRepositories;
using edumis.Models.Global;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace edumisbackend.Controllers.Visitor;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AlumniVisitorController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpPost("log")]
    public async Task<IActionResult> LogVisit()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers["User-Agent"].ToString();

        if (await unitOfWork.AlumniVisitorCounterRepo.Exists(x =>
            x.IPAddress == ipAddress && x.VisitDateTime.Date == DateTime.UtcNow.Date))
        {
            // If a visit from this IP on the same day already exists, return success without saving
            var allLogs = await unitOfWork.AlumniVisitorCounterRepo.GetAll();
            return Ok(ResponseModel<string>.Success($"{(allLogs.LongCount()):D4}", "Total visitors retrieved!", StatusCodes.Status200OK));
        }

        var saveObj = new AlumniVisitorCounterModel
        {
            IPAddress = ipAddress ?? "Unknown",
            UserAgent = userAgent ?? "Unknown"
        };

        await unitOfWork.AlumniVisitorCounterRepo.Add(saveObj);
        await unitOfWork.Save();

        var allLogsUpdated = await unitOfWork.AlumniVisitorCounterRepo.GetAll();
        return Ok(ResponseModel<string>.Success($"{(allLogsUpdated.LongCount()):D7}", "Total visitors retrieved!", StatusCodes.Status200OK));
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetVisitorCount()
    {
        var allLogsUpdated = await unitOfWork.AlumniVisitorCounterRepo.GetAll();
        return Ok(ResponseModel<string>.Success($"{(allLogsUpdated.LongCount()):D7}", "Total visitors retrieved!", StatusCodes.Status200OK));
    }
}
