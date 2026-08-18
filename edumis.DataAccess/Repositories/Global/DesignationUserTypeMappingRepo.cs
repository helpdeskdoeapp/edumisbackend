using edumis.DataAccess.IRepositories.IGlobal;
using edumis.DataAccess.Mappers;
using edumis.Models;
using edumis.Models.Global.DTO;

namespace edumis.DataAccess.Repositories.Global;

internal class DesignationUserTypeMappingRepo : Repository<Models.Global.DesignationUserTypeMapping>, IDesignationUserTypeMapping
{
    private readonly ApplicationDBContext dBContext;
    public DesignationUserTypeMappingRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<List<DesignationUserTypeMappingDetailsDTO>> GetAllMappings()
    {
        ErrorModel error = null;
        return await ExecuteSPReader("select * from spgl_getdesig_usertypemappings()", null, DesignationUserTypeMapper.ToMappings, error);
    }
}
