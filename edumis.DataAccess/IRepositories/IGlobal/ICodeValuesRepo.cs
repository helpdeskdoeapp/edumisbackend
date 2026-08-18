using edumis.Models.Global;
using edumis.Models.Global.DTO;

namespace edumis.DataAccess.IRepositories.IGlobal
{
    public interface ICodeValuesRepo : IRepository<CodeValuesModel>
    {
        Task<List<MasterCodeValueDetailsDTO>> GetAllMasterCodeValues(int id);
        Task<List<MasterCodeValueDetailsDTO>> GetAllMasterCodeValues(List<int> mastercodes);
        Task<string> GetCodeValueDescription(int CodeValue);
        Task<bool> AddNewMasterSubCode(CodeValuesModel codeValModel);
        Task<bool> Update(CodeValuesModel codeValModel);
    }
}
