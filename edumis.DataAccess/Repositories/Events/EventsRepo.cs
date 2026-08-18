using edumis.DataAccess.IRepositories.IEvents;
using edumis.Models.Events;
using edumis.Models.Events.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Events;

internal class EventsRepo(ApplicationDBContext dBContext) : Repository<EventsModel>(dBContext), IEventsRepo
{
    public async Task<IEnumerable<EventResponseDTO>?> GetAllEvents(SearchEventsRequestDTO requestDTO)
    {
        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(c => c.CodeValue, c => c.CodeValDescription);

        var eventsList = !string.IsNullOrEmpty(requestDTO.BranchId) ? await dBContext.Events
                .AsNoTracking()
                .Where(a => 
                    a.FinancialYear == requestDTO.ForSession &&
                    a.StartDate >= requestDTO.FromDate &&
                    a.StartDate <= requestDTO.ToDate &&
                    a.BranchId == requestDTO.BranchId).ToListAsync()
            :
            await dBContext.Events
                .AsNoTracking()
                .Where(a =>
                    a.FinancialYear == requestDTO.ForSession &&
                    a.StartDate >= requestDTO.FromDate &&
                    a.StartDate <= requestDTO.ToDate).ToListAsync();        

        if (eventsList == null) return null;

        var returnData = eventsList.Select(evt => new EventResponseDTO
        {
            RecordId = evt.RowId,
            Title = evt.Title,
            Description = evt.Description,
            Venue = evt.Venue,
            Category = evt.Category,
            CategoryDesc = codeValuesLookup.TryGetValue(evt.Category, out var desc) ? desc : "Unknown",
            StartDate = evt.StartDate,
            EndDate = evt.EndDate,
            StartTime = evt.StartTime,
            EndTime = evt.EndTime,
            OrganizedBy = evt.OrganizedBy,
            VideoLink = evt.VideoLink,
            ExternalLink = evt.ExternalLink,
            AlumniEvent = evt.AlumniEvent
        });

        return returnData;
    }
}
