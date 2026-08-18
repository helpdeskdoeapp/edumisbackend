using edumis.DataAccess.IRepositories.ILibrary.IMagazine;
using edumis.Models.Library.Magazine;
using edumis.Models.Library.Magazine.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Library.Magazine;

internal class MagazineRepo : Repository<MagazineModel>, IMagazineRepo
{
    private readonly ApplicationDBContext dBContext;
    public MagazineRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<MagazineDetailsReponseDTO?> GetDetails(Guid magazineId)
    {
        var branchesLookup = await dBContext.Branches
            .AsNoTracking()
            .ToDictionaryAsync(b => b.BranchId, b => b.BranchName);

        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(c => c.CodeValue, c => c.CodeValDescription);

        var magzineDetailsEntity = await dBContext.Magazines
            .AsNoTracking()          
            .Include(b => b.MagazineProcurementTransactionList)
            .FirstOrDefaultAsync(b => b.MagazineId == magazineId);

        if (magzineDetailsEntity == null) return null;

        var dto = new MagazineDetailsReponseDTO
        {
            MagazineId = magzineDetailsEntity.MagazineId,
            BranchId = magzineDetailsEntity.BranchId,
            BranchName = branchesLookup.GetValueOrDefault(magzineDetailsEntity.BranchId, string.Empty),
            Title = magzineDetailsEntity.Title,   
            Publisher = magzineDetailsEntity.Publisher,
            Editor = magzineDetailsEntity.Editor,
            Edition = magzineDetailsEntity.Edition,          
            Language = magzineDetailsEntity.Language,
            LanguageDesc = codeValuesLookup.GetValueOrDefault(magzineDetailsEntity.Language, string.Empty),
            Frequency = magzineDetailsEntity.Frequency,
            FrequencyDesc = codeValuesLookup.GetValueOrDefault(magzineDetailsEntity.Frequency, string.Empty),
            Description = magzineDetailsEntity.Description,        
            CoverImageUrl = magzineDetailsEntity.CoverImageUrl,
            CoverImageExtenstion = magzineDetailsEntity.CoverImageExtenstion,
            CoverImageContentType = magzineDetailsEntity.CoverImageContentType,
            Notes = magzineDetailsEntity.Notes,
            Tags = magzineDetailsEntity.Tags,
            Rating = magzineDetailsEntity.Rating,
            EBookUrl = magzineDetailsEntity.EBookUrl,
            AudioUrl = magzineDetailsEntity.AudioUrl,
            VideoUrl = magzineDetailsEntity.VideoUrl,     
            TotalQty = magzineDetailsEntity.TotalQty,
            AvailableQty = magzineDetailsEntity.AvailableQty,
            Genre = magzineDetailsEntity.Genre?
                .ToDictionary(id => id, id => codeValuesLookup.GetValueOrDefault(id, string.Empty)),

            RelatedMagazines = magzineDetailsEntity.RelatedMagazines != null
                ? await dBContext.Magazines
                    .AsNoTracking()
                    .Where(rb => magzineDetailsEntity.RelatedMagazines.Contains(rb.MagazineId))
                    .Select(rb => new RelatedMagazinesDetailsDTO
                    {
                        MagazineId = rb.MagazineId,
                        Title = rb.Title,                      
                        CoverImageUrl = rb.CoverImageUrl,
                        CoverImageExtenstion = rb.CoverImageExtenstion,
                        CoverImageContentType=rb.CoverImageContentType,
                        Edition = rb.Edition,
                        Editor = rb.Editor,
                        Publisher = rb.Publisher,
                        Rating = rb.Rating
                    }).ToListAsync()
                : new List<RelatedMagazinesDetailsDTO>(),           

            MagazineProcurementTransactions = magzineDetailsEntity.MagazineProcurementTransactionList?
                .Select(p => new MagazineProcurementTransactionDetailsDTO
                {
                    TransactionId = p.TransactionId,
                    ProcurementSource = p.ProcurementSource,
                    ProcurementSourceDesc = codeValuesLookup.GetValueOrDefault(p.ProcurementSource, string.Empty),
                    ProcurementDate = p.ProcurementDate,
                    OtherProcurementSource = p.OtherProcurementSource,
                    BillNo = p.BillNo,
                    BillDate = p.BillDate,
                    BillAmount = p.BillAmount,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToList()
        };

        return dto;
    }

    public async Task<IEnumerable<MagazineDetailsReponseDTO>?> GetMagazines(string branchId)
    {
        var branchesLookup = await dBContext.Branches
            .AsNoTracking()
            .ToDictionaryAsync(b => b.BranchId, b => b.BranchName);

        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(c => c.CodeValue, c => c.CodeValDescription);

        var magazineDetailsEntity = await dBContext.Magazines
             .AsNoTracking()            
             .Where(b => b.BranchId == branchId).ToListAsync();

        if (magazineDetailsEntity == null) return null;

        var returnData = magazineDetailsEntity
            .Select(magzineDetailsEntity => new MagazineDetailsReponseDTO
            {
                MagazineId = magzineDetailsEntity.MagazineId,
                BranchId = magzineDetailsEntity.BranchId,
                BranchName = branchesLookup.GetValueOrDefault(magzineDetailsEntity.BranchId, string.Empty),
                Title = magzineDetailsEntity.Title,
                Publisher = magzineDetailsEntity.Publisher,
                Editor = magzineDetailsEntity.Editor,
                Edition = magzineDetailsEntity.Edition,
                Language = magzineDetailsEntity.Language,
                LanguageDesc = codeValuesLookup.GetValueOrDefault(magzineDetailsEntity.Language, string.Empty),
                Description = magzineDetailsEntity.Description,
                CoverImageUrl = magzineDetailsEntity.CoverImageUrl,
                CoverImageExtenstion = magzineDetailsEntity.CoverImageExtenstion,
                CoverImageContentType = magzineDetailsEntity.CoverImageContentType,
                Notes = magzineDetailsEntity.Notes,
                Tags = magzineDetailsEntity.Tags,
                Rating = magzineDetailsEntity.Rating,
                EBookUrl = magzineDetailsEntity.EBookUrl,
                AudioUrl = magzineDetailsEntity.AudioUrl,
                VideoUrl = magzineDetailsEntity.VideoUrl,
                TotalQty = magzineDetailsEntity.TotalQty,
                AvailableQty = magzineDetailsEntity.AvailableQty,
                Genre = magzineDetailsEntity.Genre?
                .ToDictionary(id => id, id => codeValuesLookup.GetValueOrDefault(id, string.Empty)),
                Frequency = magzineDetailsEntity.Frequency,
                FrequencyDesc = codeValuesLookup.GetValueOrDefault(magzineDetailsEntity.Frequency, string.Empty)
            }).ToList();

        return returnData;
    }
}
