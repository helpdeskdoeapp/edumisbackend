using AutoMapper;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;

namespace edumisbackend.Mappers.SMC;

public class SMCBudgetMapper: Profile {
    public SMCBudgetMapper() {
        CreateMap<SmcBudgetAllocationHistoryModel, SmcBudgetHistoryEntry>();
    }
    
}