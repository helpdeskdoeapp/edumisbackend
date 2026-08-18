using edumis.DataAccess.IRepositories.IAlumni;
using edumis.Models.Alumni.Members;
using edumis.Models.Alumni.Members.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Alumni;

internal class AlumniDetailsRepo : Repository<AlumniDetailsModel>, IAlumniDetailsRepo
{

    private readonly ApplicationDBContext dBContext;
    public AlumniDetailsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<List<SelectedAlumniCollageDTO>?> GetCollageAlumni(int fetchRecords)
    {
        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(x => x.CodeValue, x => x.CodeValDescription);

        var branchesLookup = await dBContext.Branches.AsNoTracking()
       .ToDictionaryAsync(x => x.BranchId, x => x.BranchName);

        var alumniQuery = dBContext.AlumniDetails.Where(x => x.ShowOnHomePage).AsNoTracking().AsQueryable();

        if (alumniQuery == null || alumniQuery.Count() == 0) return null;

        var returnData = await alumniQuery
            .OrderBy(x => Guid.NewGuid())
            .Take(fetchRecords)
            .Select(x => new SelectedAlumniCollageDTO
            {
                BranchId = x.BranchId,
                BranchName = x.BranchNotInList ?
                x.OtherBranchName :
                branchesLookup.GetValueOrDefault(x.BranchId ?? string.Empty, string.Empty),
                Name = string.Join(" ", new[] { x.FirstName, x.MiddleName, x.LastName }
                        .Where(s => !string.IsNullOrEmpty(s))),
                ExitYear = x.ExitYear,
                ProfileImage = x.ProfileImage,
                ProfileImageContentType = x.ProfileImageContentType,
                ProfileImageExtn = x.ProfileImageExtn,
                RegistrationYear = x.RegistrationYear,
                SalutationTitle = codeValuesLookup.GetValueOrDefault(x.Salutation, string.Empty),
                ImageUrl = x.ImageUrl
            }).ToListAsync();

        return returnData;
    }

    public async Task<AlumniDetailsDTO?> GetDetails(Guid alumniId)
    {
        return await (from a in dBContext.AlumniDetails
               join st in dBContext.AlumniInformationShare on a.AlumniId equals st.AlumniID
               join b in dBContext.CodeValues on a.Salutation equals b.CodeValue
               join d in dBContext.CodeValues on a.Gender equals d.CodeValue
               join c in dBContext.CodeValues on a.CurrentProfession equals c.CodeValue into currentProfession
               from profession in currentProfession.DefaultIfEmpty()
               join e in dBContext.Branches on a.BranchId equals e.BranchId into branch
               from br in branch.DefaultIfEmpty()
               where a.AlumniId == alumniId
               select new AlumniDetailsDTO
               {
                   DOERegistrationId = a.DOERegistrationId,
                   Salutation = a.Salutation,
                   SalutationTitle = b.CodeValDescription,
                   FirstName = a.FirstName,
                   LastName = a.LastName,
                   MiddleName = a.MiddleName,
                   DOB = a.DOB,
                   Gender = a.Gender,
                   GenderTitle = d.CodeValDescription,
                   RegistrationYear = a.RegistrationYear,
                   ExitYear = a.ExitYear,
                   BranchId = a.BranchId,
                   BranchName = br.BranchName,
                   BranchNotInList = a.BranchNotInList,
                   OtherBranchName = a.OtherBranchName,
                   EmailID = a.EmailID,
                   AlternateEmailId = a.AlternateEmailId,
                   CurrentOrganization = a.CurrentOrganization,
                   CurrentDesignation = a.CurrentDesignation,
                   CurrentResidence = a.CurrentResidence,
                   ResidenceContactNo = a.ResidenceContactNo,
                   WorkContactNo = a.WorkContactNo,
                   MobileNo = a.MobileNo,
                   CurrentResidenceCity = a.CurrentResidenceCity,
                   CurrentProfession = a.CurrentProfession,
                   CurrentProfessionDesc = profession.CodeValDescription,
                   OtherProfession = a.OtherProfession,
                   IsResidentOfDelhi = a.IsResidentOfDelhi,
                   ProfileImage = a.ProfileImage,
                   ProfileImageExtn = a.ProfileImageExtn,
                   ProfileImageContentType = a.ProfileImageContentType,
                   ShowEmailID = st.EmailID,
                   ShowMobileNo = st.MobileNo,
                   ShowCurrentOrganisation = st.CurrentOrganisation,
                   ShowCurrentDesignation = st.CurrentDesignation,
                   ShowCurrentResidence = st.CurrentResidence,
                   ShowResidenceContactNo = st.ResidenceContactNo,
                   ShowWorkContactNo = st.WorkContactNo,
                   ShowCurrentResidenceCity = st.CurrentResidenceCity,
                   ShowCurrentProfession = st.CurrentProfession,
                   ShowOnHomePage = a.ShowOnHomePage,
                   ImageUrl = a.ImageUrl
               }).FirstOrDefaultAsync();
    }

    public Task RegisterBranchAlumni(SchoolAlumniRegistrationRequestDTO requestDTO, string registeredBy)
    {
        throw new NotImplementedException();
    }

    public async Task<List<AlumniSearchResponseDTO>?> Search(AlumniSearchRequestDTO requestDTO)
    {       
        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()           
            .ToDictionaryAsync(x => x.CodeValue, x => x.CodeValDescription);

        var branchesLookup = await dBContext.Branches.AsNoTracking()      
        .ToDictionaryAsync(x => x.BranchId, x => x.BranchName);

        var alumniQuery = dBContext.AlumniDetails.AsNoTracking().AsQueryable();

        if (requestDTO.Gender.HasValue && requestDTO.Gender.Value != 0)
            alumniQuery = alumniQuery.Where(x => x.Gender == requestDTO.Gender.Value);

        if (requestDTO.CurrentProfession.HasValue && requestDTO.CurrentProfession.Value != 0)
            alumniQuery = alumniQuery.Where(x => x.CurrentProfession == requestDTO.CurrentProfession.Value);

        if (requestDTO.District.HasValue && requestDTO.District.Value != 0)
        {
            var zoneIds = await dBContext.Zones
                .Where(z => z.DistrictId == requestDTO.District.Value)
                .Select(z => z.RowId)
                .ToListAsync();

            if (requestDTO.Zone.HasValue && requestDTO.Zone.Value != 0)
                zoneIds = zoneIds.Where(zid => zid == requestDTO.Zone.Value).ToList();

            var branchIds = await dBContext.Branches
                .Where(b => zoneIds.Contains(b.ZoneId ?? 0))
                .Select(b => b.BranchId)
                .ToListAsync();

            alumniQuery = alumniQuery.Where(a => a.BranchId != null && branchIds.Contains(a.BranchId));
        }       
        
        var returnData = await alumniQuery.Select(x => new AlumniSearchResponseDTO
        {
            AlumniId = x.AlumniId,
            BranchId = x.BranchId,
            BranchName = x.BranchId != null ? branchesLookup.GetValueOrDefault(x.BranchId, string.Empty) : string.Empty,
            FirstName = x.FirstName,
            LastName = x.LastName,
            MiddleName = x.MiddleName,
            BranchNotInList = x.BranchNotInList,
            OtherBranchName = x.OtherBranchName,
            DOB = x.DOB,
            DOERegistrationId = x.DOERegistrationId,
            EmailID = x.EmailID,
            ExitYear = x.ExitYear,
            GenderTitle = codeValuesLookup.GetValueOrDefault(x.Gender, string.Empty),
            IsResidentOfDelhi = x.IsResidentOfDelhi,
            MobileNo = x.MobileNo,
            ProfileImage = x.ProfileImage,
            ProfileImageContentType = x.ProfileImageContentType,
            ProfileImageExtn = x.ProfileImageExtn,
            RegistrationYear = x.RegistrationYear,            
            SalutationTitle = codeValuesLookup.GetValueOrDefault(x.Salutation, string.Empty),
            CurrentProfession = x.CurrentProfession != null ? codeValuesLookup.GetValueOrDefault(x.CurrentProfession.Value, string.Empty) : string.Empty,
            ShowOnHomePage = x.ShowOnHomePage,
            ImageUrl = x.ImageUrl
        }).ToListAsync();
        
        return returnData;
    }

    public async Task<List<AlumniSearchResponseDTO>?> Search(string searchText)
    {
        var codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(x => x.CodeValue, x => x.CodeValDescription);

        var branchesLookup = await dBContext.Branches.AsNoTracking()
       .ToDictionaryAsync(x => x.BranchId, x => x.BranchName);

        var alumniQuery = dBContext.AlumniDetails.AsNoTracking().AsQueryable();       
                      
        var returnData = alumniQuery.Select(x => new AlumniSearchResponseDTO
        {
            AlumniId = x.AlumniId,
            BranchId = x.BranchId,
            BranchName = branchesLookup.GetValueOrDefault(x.BranchId ?? string.Empty, string.Empty),
            FirstName = x.FirstName,
            LastName = x.LastName,
            MiddleName = x.MiddleName,
            BranchNotInList = x.BranchNotInList,
            OtherBranchName = x.OtherBranchName,
            DOB = x.DOB,
            DOERegistrationId = x.DOERegistrationId,
            EmailID = x.EmailID,
            ExitYear = x.ExitYear,
            GenderTitle = codeValuesLookup.GetValueOrDefault(x.Gender, string.Empty),
            IsResidentOfDelhi = x.IsResidentOfDelhi,
            MobileNo = x.MobileNo,
            ProfileImage = x.ProfileImage,
            ProfileImageContentType = x.ProfileImageContentType,
            ProfileImageExtn = x.ProfileImageExtn,
            RegistrationYear = x.RegistrationYear,
            SalutationTitle = codeValuesLookup.GetValueOrDefault(x.Salutation, string.Empty),
            CurrentProfession = x.CurrentProfession != null ? codeValuesLookup.GetValueOrDefault(x.CurrentProfession.Value, string.Empty) : string.Empty,
            ShowOnHomePage = x.ShowOnHomePage,
            ImageUrl = x.ImageUrl
        });

        return await returnData.ToListAsync();
    }
}
