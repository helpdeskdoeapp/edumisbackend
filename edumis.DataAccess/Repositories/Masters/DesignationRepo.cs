using edumis.DataAccess.IRepositories.IMasters;
using edumis.Models.Masters.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace edumis.DataAccess.Repositories.Masters;

internal class DesignationRepo : Repository<Models.Masters.DesignationModel>, IDesignationRepo
{
    private readonly ApplicationDBContext dBContext;
    public DesignationRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<bool> Update(DesignationUpdateRequestDTO requestDTO, string userId)
    {
        var rowsAffected = await dBContext.Designations.Where(x => x.RowId == requestDTO.DesignationId).ExecuteUpdateAsync(b => b
            .SetProperty(prop => prop.Title, requestDTO.Title)
            .SetProperty(prop => prop.DesignationGroup, requestDTO.DesignationGroup)
            .SetProperty(prop => prop.IsGazetted, requestDTO.IsGazetted)
            .SetProperty(prop => prop.ModifiedBy, userId)
            .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
        );
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateStatus(int designationId, bool status, string userId)
    {
        var rowsAffected = await dBContext.Designations.Where(x => x.RowId == designationId).ExecuteUpdateAsync(b => b            
            .SetProperty(prop => prop.IsActive, status)
            .SetProperty(prop => prop.ModifiedBy, userId)
            .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
        );
        return rowsAffected > 0;
    }

    public async Task<List<DesignationDetailsDTO>?> GetDesignations()
    {
        return await (from a in dBContext.Designations
        join b in dBContext.CodeValues on a.DesignationGroup equals b.CodeValue
        select new DesignationDetailsDTO
        (   
            a.RowId,
            a.Title, 
            a.DesignationGroup, 
            b.CodeValDescription, 
            a.IsGazetted, 
            a.IsActive
        )).ToListAsync();
    }    
}
