using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.IGlobal;
using edumis.Models;
using edumis.Models.Global;
using edumis.Models.Global.DTO;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Global;

internal class CodeValuesRepo : Repository<edumis.Models.Global.CodeValuesModel>, ICodeValuesRepo
{
    private readonly ApplicationDBContext dBContext;
    public CodeValuesRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<bool> AddNewMasterSubCode(CodeValuesModel codeValModel)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "pcode", DBType= NpgsqlDbType.Integer, ParamValue = codeValModel.Code },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "pcodevalue", DBType= NpgsqlDbType.Integer, ParamValue = codeValModel.CodeValue },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "pcodevaldescription", ParamValue = codeValModel.CodeValDescription},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "pparentcode", DBType= NpgsqlDbType.Integer, ParamValue = codeValModel.ParentCode == null ? 0 : codeValModel.ParentCode},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "pisactive",DBType= NpgsqlDbType.Boolean, ParamValue = codeValModel.IsActive},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "puserid",DBType= NpgsqlDbType.Uuid, ParamValue = string.IsNullOrEmpty(codeValModel.CreatedBy)? new Guid() : new Guid(codeValModel.CreatedBy) }
            };
        ErrorModel error = null;
        return await ExecStoredProcedureWithTrans(@"call spgl_codevalinsupd(:pcode,
                            :pcodevalue,
                            :pcodevaldescription,
                            :pparentcode,
                            :pisactive,
                            :puserid
                        )", spParamList, error);
    }

    public async Task<List<MasterCodeValueDetailsDTO>> GetAllMasterCodeValues(int code)
    {
        List<MasterCodeValueDetailsDTO> ReturnData = null;

        var CodeValues = await dBContext.CodeValues.Where(x => x.Code == code).ToListAsync();

        if (CodeValues == null) return ReturnData;

        ReturnData = new List<MasterCodeValueDetailsDTO>();
        foreach (var CodeValue in CodeValues)
        {
            MasterCodeValueDetailsDTO valueDTO = new MasterCodeValueDetailsDTO();

            valueDTO.Code = CodeValue.Code;
            valueDTO.SubCode = CodeValue.CodeValue;
            valueDTO.SubCodeDescription = CodeValue.CodeValDescription;
            valueDTO.ParentCode = CodeValue.ParentCode;
            valueDTO.IsActive = CodeValue.IsActive;

            ReturnData.Add(valueDTO);
        }

        return ReturnData;
    }

    public async Task<List<MasterCodeValueDetailsDTO>> GetAllMasterCodeValues(List<int> mastercodes)
    {
        var CodeValues = dBContext.CodeValues.AsEnumerable()
                         .Join(mastercodes, x => x.Code, y => y, (x, y) => x)
                         .OrderBy(x => x.Code)
                         .OrderBy(x => x.CodeValue)
                         .Select(codes => new MasterCodeValueDetailsDTO()
                         {
                             Code = codes.Code,
                             SubCode = codes.CodeValue,
                             SubCodeDescription = codes.CodeValDescription,
                             ParentCode = codes.ParentCode,
                             IsActive = codes.IsActive
                         }).ToList();

        return CodeValues;
    }

    public async Task<string> GetCodeValueDescription(int CodeValue)
    {
        var SelectedCodeValue = await dBContext.CodeValues.Where(x => x.CodeValue == CodeValue).FirstOrDefaultAsync();

        if (SelectedCodeValue == null)
            return string.Empty;

        return SelectedCodeValue.CodeValDescription;
    }

    public async Task<bool> Update(CodeValuesModel codeValModel)
    {
        var selectedCode = await dBContext.CodeValues.Where(x => x.CodeValue == codeValModel.CodeValue).FirstOrDefaultAsync();
        if (selectedCode == null)
            return false;

        selectedCode.CodeValDescription = codeValModel.CodeValDescription;
        selectedCode.ParentCode = codeValModel.ParentCode;
        selectedCode.IsActive = codeValModel.IsActive;
        selectedCode.ModifiedDate = DateTime.Now.Date;
        selectedCode.ModifiedBy = codeValModel.ModifiedBy;

        dBContext.Entry<CodeValuesModel>(selectedCode).CurrentValues.SetValues(codeValModel);

        return true;
    }
}
