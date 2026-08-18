using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Masters.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace edumisbackend.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfrastructureController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        
        public InfrastructureController(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;            
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] InfrastructureDTO infrastructureDetails)
        {
            if (infrastructureDetails == null) return BadRequest();

            try
            {
                var returnval = await UnitOfWork.Infrastructures.CreateOrUpdate(infrastructureDetails, true);
                if (string.IsNullOrEmpty(returnval))
                    return BadRequest(new ResponseModel()
                    {
                        ReturnId = string.Empty,
                        Message = "Invalid/Bad Request Details",
                        Success = false,
                        ReturnCode = StatusCodes.Status400BadRequest.ToString()
                    });

                return Ok(new ResponseModel()
                {
                    ReturnId = returnval,
                    Message = "Infrastructure Details Saved Successfully.",
                    Success = true,
                    ReturnCode = StatusCodes.Status201Created.ToString()
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("update/{buildingid}")]
        public async Task<ActionResult> Update([FromRoute] string buildingid, [FromBody] InfrastructureDTO infrastructureDetails)
        {
            if (buildingid != infrastructureDetails.BuildingId) return BadRequest();
            if (string.IsNullOrEmpty(infrastructureDetails.BuildingId)) return BadRequest();

            if (await UnitOfWork.Infrastructures.GetFirstOrDefault(x => x.BuildingId == buildingid) == null) return BadRequest();

            try
            {
                string returnval = await UnitOfWork.Infrastructures.CreateOrUpdate(infrastructureDetails);
                if (string.IsNullOrEmpty(returnval))
                    return BadRequest(new ResponseModel()
                    {
                        ReturnId = buildingid,
                        Message = "Failed To Update Infrastructure Details.",
                        Success = false,
                        ReturnCode = StatusCodes.Status400BadRequest.ToString()
                    });

                return Ok(new ResponseModel()
                {
                    ReturnId = buildingid,
                    Message = "Infrastructure Details Updated Successfully.",
                    Success = true,
                    ReturnCode = StatusCodes.Status200OK.ToString()
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("buildingdetails/{buildingid}")]
        public async Task<ActionResult<InfrastructureDetailsDTO>> GetBuildingById([FromRoute] string buildingid)
        {
            var ReturnData = await UnitOfWork.Infrastructures.GetFirstOrDefault(x => x.BuildingId == buildingid);
            if (ReturnData == null)
                return NotFound();

            return Ok(new InfrastructureDetailsDTO(
                        ReturnData.BuildingId,
                        ReturnData.BuildingName,
                        ReturnData.Location,
                        ReturnData.Longitude,
                        ReturnData.Latitude,
                        ReturnData.LandOwning,
                        string.Empty,
                        ReturnData.TotalFloors,
                        ReturnData.TotalArea,
                        ReturnData.Fencing,
                        ReturnData.TinShed,
                        ReturnData.park,
                        ReturnData.TotalTrees,
                        ReturnData.WaterHarvesting,
                        ReturnData.DrinkingWater,
                        ReturnData.ToiletFacility,
                        ReturnData.HandicapRamp,
                        ReturnData.CycleStand,
                        ReturnData.VehicleParking,
                        ReturnData.Accommodation,
                        ReturnData.BadmintonCourt,
                        ReturnData.TTHall,
                        ReturnData.BasketBallCourt,
                        ReturnData.ShootingRange,
                        ReturnData.SwimmingPool,
                        ReturnData.BoxingArena,
                        ReturnData.WrestlingArena,
                        ReturnData.RunningTrack,
                        ReturnData.WeightLiftingHall,
                        ReturnData.LawnTenisCourt,
                        ReturnData.ArcheryGround,
                        ReturnData.OpeningYear,
                        ReturnData.MaintenanceAgency,
                        ReturnData.IsActive
                    ));
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<InfrastructureDetailsDTO>>> GetAllInfra()
        {
            var ReturnData = await UnitOfWork.Infrastructures.GetAllInfra();

            if (ReturnData == null)
                return BadRequest(new ResponseModel()
                {
                    Success = false,
                    Message = "No Data Found.",
                    ReturnCode = StatusCodes.Status400BadRequest.ToString()
                });

            return Ok(ReturnData);
        }
    }
}
