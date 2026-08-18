using edumis.DataAccess.IRepositories.ILibrary.INewsPaper;
using edumis.Models.Library.Newspaper;
using edumis.Models.Library.Newspaper.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Library.Newspaper;

internal class NewspaperRepo : Repository<NewspaperModel>, INewspaperRepo
{
    private readonly ApplicationDBContext dBContext;
    public NewspaperRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<NewspaperDetailsResponseDTO?> GetDetails(Guid recordId)
    { 
        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(c => c.CodeValue, c => c.CodeValDescription);

        var newsDetailsEntity = await dBContext.Newspapers
            .AsNoTracking()  
            .FirstOrDefaultAsync(b => b.NewspaperId == recordId);

        var branchesLookup = await dBContext.Branches
            .AsNoTracking()
            .ToDictionaryAsync(b => b.BranchId, b => b.BranchName);

        if (newsDetailsEntity == null) return null;

        var dto = new NewspaperDetailsResponseDTO
        {
            NewspaperId = newsDetailsEntity.NewspaperId,
            BranchId = newsDetailsEntity.BranchId,
            BranchName = branchesLookup.GetValueOrDefault(newsDetailsEntity.BranchId, string.Empty),
            Title = newsDetailsEntity.Title,
            Language = newsDetailsEntity.Language,
            LanguageDesc = codeValuesLookup.GetValueOrDefault(newsDetailsEntity.Language, string.Empty),
            Frequency = newsDetailsEntity.Frequency,
            FrequencyDesc = codeValuesLookup.GetValueOrDefault(newsDetailsEntity.Frequency, string.Empty),
            Genre = newsDetailsEntity.Genre?
                .ToDictionary(id => id, id => codeValuesLookup.GetValueOrDefault(id, string.Empty)),
            Description = newsDetailsEntity.Description,
            EBookUrl = newsDetailsEntity.EBookUrl,
            Price = newsDetailsEntity.Price,
            Quantity = newsDetailsEntity.Quantity,
            IsActive = newsDetailsEntity.IsActive
        };

        return dto;
    }

    public async Task<IEnumerable<NewspaperDetailsResponseDTO>?> GetNewspapers(string branchId)
    {
        var branchesLookup = await dBContext.Branches.Where(x=>x.BranchId == branchId)
            .AsNoTracking()
            .ToDictionaryAsync(b => b.BranchId, b => b.BranchName);

        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(c => c.CodeValue, c => c.CodeValDescription);

        var newsEntity = await dBContext.Newspapers
             .AsNoTracking()             
             .Where(b => b.BranchId == branchId).ToListAsync();

        if (newsEntity == null) return null;

        var returnData = newsEntity
            .Select(news => new NewspaperDetailsResponseDTO
            {
                NewspaperId = news.NewspaperId,
                BranchId = news.BranchId,
                BranchName = branchesLookup.GetValueOrDefault(news.BranchId, string.Empty),
                Title = news.Title,
                Language = news.Language,
                LanguageDesc = codeValuesLookup.GetValueOrDefault(news.Language, string.Empty),
                Frequency = news.Frequency,
                FrequencyDesc = codeValuesLookup.GetValueOrDefault(news.Frequency, string.Empty),
                Genre = news.Genre?
                .ToDictionary(id => id, id => codeValuesLookup.GetValueOrDefault(id, string.Empty)),
                Description = news.Description,
                EBookUrl = news.EBookUrl,
                Price = news.Price,
                Quantity = news.Quantity,
                IsActive = news.IsActive
            }).ToList();

        return returnData;
    }
}
