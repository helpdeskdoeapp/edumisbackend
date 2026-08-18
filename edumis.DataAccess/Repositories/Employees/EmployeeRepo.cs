using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.IEmployees;
using edumis.DataAccess.Mappers.Employees;
using edumis.Models;
using edumis.Models.Employees.DTO;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System.Data;
using edumis.DataAccess.Extensions;

namespace edumis.DataAccess.Repositories.Employees;

internal class EmployeeRepo : Repository<edumis.Models.Employees.EmployeeModel>, IEmployeeRepo
{
    private readonly ApplicationDBContext dBContext;

    public EmployeeRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    #region Create Employee

    public async Task<string?> CreateEmployee(EmployeeDTO empModel, string CreatedBy)
    {
        var spParamList = new ParamHelper
        {
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_firstname", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.FirstName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_middlename", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.MiddleName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_lastname", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.LastName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_fathername", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.FatherName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_mothername", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.MotherName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_gender", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.Gender
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_dob", DBType = NpgsqlDbType.Date,
                ParamValue = empModel.DOB
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_aadharno", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.AadharNo
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_panno", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.PanNo
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_emailid", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.EmailId
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_mobileno", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.MobileNo
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_permanentaddress",
                DBType = NpgsqlDbType.Varchar, ParamValue = empModel.PermanentAddress
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_pcity", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.PAddressCity
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_pstate", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.PAddressState
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_ppincode", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.PAddressPincode
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_correspondenceaddress",
                DBType = NpgsqlDbType.Varchar, ParamValue = empModel.CorrespondenceAddress
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_ccity", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.CAddressCity
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_cstate", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.CAddressState
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_cpincode", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.CAddressPincode
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_category", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.Category
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_subcategory", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.SubCategory
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_highestqualification",
                DBType = NpgsqlDbType.Integer, ParamValue = empModel.HighestQualification
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_maritalstatus", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.MaritalStatus
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_isanydisability",
                DBType = NpgsqlDbType.Boolean, ParamValue = empModel.IsAnyDisability
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_disabilitytype",
                DBType = NpgsqlDbType.Integer, ParamValue = empModel.DisabilityType
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_otherdisabilitytype",
                DBType = NpgsqlDbType.Varchar, ParamValue = empModel.OtherDisabilityType
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_isgazetted", DBType = NpgsqlDbType.Boolean,
                ParamValue = empModel.IsGazetted
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_vehiclefacilityavailed",
                DBType = NpgsqlDbType.Boolean, ParamValue = empModel.VehicleFacilityAvailed
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_reportingpersonid",
                DBType = NpgsqlDbType.Varchar,
                ParamValue = !string.IsNullOrEmpty(empModel.ReportingPersonId) ? empModel.ReportingPersonId : ""
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_isactive", DBType = NpgsqlDbType.Boolean,
                ParamValue = empModel.IsActive
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_remarks", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.Remarks
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType = NpgsqlDbType.Varchar,
                ParamValue = CreatedBy
            }
        };

        ErrorModel? error = null;
        object? ReturnVal = await ExecNonQueryTransSingle(
            @"select * from spemp_createemployee(                                                                    
                                                                    @p_firstname,
                                                                    @p_middlename,
                                                                    @p_lastname,
                                                                    @p_fathername,
                                                                    @p_mothername,
                                                                    @p_gender,
                                                                    @p_dob,
                                                                    @p_aadharno,
                                                                    @p_panno,
                                                                    @p_emailid,
                                                                    @p_mobileno,
                                                                    @p_permanentaddress,
                                                                    @p_pcity,
                                                                    @p_pstate,
                                                                    @p_ppincode,
                                                                    @p_correspondenceaddress,
                                                                    @p_ccity,
                                                                    @p_cstate,
                                                                    @p_cpincode,
                                                                    @p_category,
                                                                    @p_subcategory,                                                                   
                                                                    @p_highestqualification,
                                                                    @p_maritalstatus,
                                                                    @p_isanydisability,
                                                                    @p_disabilityType,
                                                                    @p_otherdisabilityType,
                                                                    @p_isgazetted,
                                                                    @p_vehiclefacilityavailed,
                                                                    @p_reportingpersonid,
                                                                    @p_isactive,
                                                                    @p_remarks,                                                                     
                                                                    @p_userid)", spParamList, error);

        if (ReturnVal != null && !string.IsNullOrEmpty(ReturnVal.ToString()))
        {
            spParamList = new ParamHelper
            {
                new NpgSqlParam
                {
                    ParamDirection = ParameterDirection.Input, ParamName = "p_employeeid",
                    DBType = NpgsqlDbType.Varchar, ParamValue = ReturnVal.ToString()
                },
                new NpgSqlParam
                {
                    ParamDirection = ParameterDirection.Input, ParamName = "p_password", DBType = NpgsqlDbType.Varchar,
                    ParamValue = Common.Utilities.HashPassword(ReturnVal.ToString())
                }
            };
            await ExecStoredProcedureWithTrans(@"call spms_updatenewemppassword(:p_employeeid, :p_password)",
                spParamList, error);

            return ReturnVal.ToString();
        }

        return string.Empty;
    }

    public async Task<bool> UpdateEmployee(EmployeeDTO empModel, string UpdatedBy)
    {
        var spParamList = new ParamHelper
        {
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_employeeid", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.EmployeeId
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_FirstName", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.FirstName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_MiddleName", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.MiddleName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_LastName", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.LastName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_FatherName", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.FatherName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_MotherName", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.MotherName
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_Gender", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.Gender
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_DOB", DBType = NpgsqlDbType.Date,
                ParamValue = empModel.DOB
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_AadharNo", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.AadharNo
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_PanNo", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.PanNo
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_EmailId", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.EmailId
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_MobileNo", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.MobileNo
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_PermanentAddress",
                DBType = NpgsqlDbType.Varchar, ParamValue = empModel.PermanentAddress
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_PAddressCity", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.PAddressCity
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_PAddressState", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.PAddressState
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_PAddressPincode",
                DBType = NpgsqlDbType.Varchar, ParamValue = empModel.PAddressPincode
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_CorrespondenceAddress",
                DBType = NpgsqlDbType.Varchar, ParamValue = empModel.CorrespondenceAddress
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_CAddressCity", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.CAddressCity
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_CAddressState", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.CAddressState
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_CAddressPincode",
                DBType = NpgsqlDbType.Varchar, ParamValue = empModel.CAddressPincode
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_Category", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.Category
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_SubCategory", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.SubCategory
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_HighestQualification",
                DBType = NpgsqlDbType.Integer, ParamValue = empModel.HighestQualification
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_MaritalStatus", DBType = NpgsqlDbType.Integer,
                ParamValue = empModel.MaritalStatus
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_IsAnyDisability",
                DBType = NpgsqlDbType.Boolean, ParamValue = empModel.IsAnyDisability
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_DisabilityType",
                DBType = NpgsqlDbType.Integer, ParamValue = empModel.DisabilityType
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_OtherDisabilityType",
                DBType = NpgsqlDbType.Varchar, ParamValue = empModel.OtherDisabilityType
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_IsGazetted", DBType = NpgsqlDbType.Boolean,
                ParamValue = empModel.IsGazetted
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_VehicleFacilityAvailed",
                DBType = NpgsqlDbType.Boolean, ParamValue = empModel.VehicleFacilityAvailed
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_ReportingPersonId",
                DBType = NpgsqlDbType.Varchar,
                ParamValue = !string.IsNullOrEmpty(empModel.ReportingPersonId) ? empModel.ReportingPersonId : ""
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_IsActive", DBType = NpgsqlDbType.Boolean,
                ParamValue = empModel.IsActive
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_Remarks", DBType = NpgsqlDbType.Varchar,
                ParamValue = empModel.Remarks
            },
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType = NpgsqlDbType.Varchar,
                ParamValue = UpdatedBy
            }
        };

        ErrorModel? error = null;
        return await ExecStoredProcedureWithTrans(@"call spemp_updateemployee(
                                                                    :p_employeeid,
                                                                    :p_FirstName,
                                                                    :p_MiddleName,
                                                                    :p_LastName,
                                                                    :p_FatherName,
                                                                    :p_MotherName,
                                                                    :p_Gender,
                                                                    :p_DOB,
                                                                    :p_AadharNo,
                                                                    :p_PanNo,
                                                                    :p_EmailId,
                                                                    :p_MobileNo,
                                                                    :p_PermanentAddress,
                                                                    :p_PAddressCity,
                                                                    :p_PAddressState,
                                                                    :p_PAddressPincode,
                                                                    :p_CorrespondenceAddress,
                                                                    :p_CAddressCity,
                                                                    :p_CAddressState,
                                                                    :p_CAddressPincode,
                                                                    :p_Category,
                                                                    :p_SubCategory,                                                                    
                                                                    :p_HighestQualification,
                                                                    :p_MaritalStatus,
                                                                    :p_IsAnyDisability,
                                                                    :p_DisabilityType,
                                                                    :p_OtherDisabilityType,
                                                                    :p_IsGazetted,
                                                                    :p_VehicleFacilityAvailed,
                                                                    :p_ReportingPersonId,
                                                                    :p_IsActive,
                                                                    :p_Remarks,                                                                    
                                                                    :p_userid)", spParamList, error);
    }

    public async Task<EmployeeDetailsDTO?> GetEmployeeDetails(string EmployeeID)
    {
        var spParamList = new ParamHelper
        {
            new NpgSqlParam
            {
                ParamDirection = ParameterDirection.Input, ParamName = "p_employeeid", DBType = NpgsqlDbType.Varchar,
                ParamValue = EmployeeID
            }
        };

        ErrorModel error = null;
        var ReturnDetails = await ExecuteSPReader("select * from spemp_getemployeedetails(:p_employeeid)", spParamList,
            EmployeeMapper.ToEmployeeDetails, error);
        return ReturnDetails.Any() ? ReturnDetails.FirstOrDefault() : null;
    }

    public async Task<List<SearchResultResponseDTO>?> SearchEmployees(SearchEmployeeRequestDTO searchEmployee)
    {
        //var branchesLookup = await dBContext.Branches
        //   .AsNoTracking()
        //   .ToDictionaryAsync(b => b.BranchId, b => b.BranchName);

        Dictionary<int, string> codeValuesLookup = await dBContext.CodeValues
            .AsNoTracking()
            .ToDictionaryAsync(c => c.CodeValue, c => c.CodeValDescription);


        var EmployeeEntityList = await (
            from a in dBContext.Employees
            join b in dBContext.EmployeeAppointmentDetails on a.EmployeeId equals b.EmployeeId
            join c in dBContext.Designations on b.Designation equals c.RowId
                into Designations
            from desig in Designations.DefaultIfEmpty()
            where b.CurrentBranch == searchEmployee.BranchId
            select new SearchResultResponseDTO()
            {
                EmployeeId = a.EmployeeId,
                FirstName = a.FirstName,
                MiddleName = a.MiddleName,
                LastName = a.LastName,
                FatherName = a.FatherName,
                MotherName = a.MotherName,
                Gender = a.Gender,
                DOB = a.DOB,
                AadharNo = a.AadharNo,
                PanNo = a.PanNo,
                EmailId = a.EmailId,
                MobileNo = a.MobileNo,
                PermanentAddress = a.PermanentAddress,
                PAddressCity = a.PCity,
                PAddressState = a.PState,
                PAddressPincode = a.PPincode,
                CorrespondenceAddress = a.CorrespondenceAddress,
                CAddressCity = a.CCity,
                CAddressState = a.CState,
                CAddressPincode = a.CPincode,
                Category = a.Category,
                SubCategory = a.SubCategory,
                SelectionCategory = b.SelectionCategory,
                HighestQualification = a.HighestQualification,
                MaritalStatus = a.MaritalStatus,
                IsAnyDisability = a.IsAnyDisability ?? false,
                DisabilityType = a.DisabilityType,
                OtherDisabilityType = a.OtherDisabilityType,
                IsGazetted = a.IsGazetted,
                VehicleFacilityAvailed = a.VehicleFacilityAvailed,
                ReportingPersonId = a.ReportingPersonId,
                DesignationId = b.Designation,
                DesignationTitle = desig.Title,
                DesignationGroup = desig.DesignationGroup,
                IsActive = a.IsActive ?? false,
                Remarks = a.Remarks,
                Photo = a.Photo,
                Extension = a.Extension,
                ContentType = a.ContentType
            }).AsNoTracking().ToListAsync();

        var returnData = EmployeeEntityList.Select(e => new SearchResultResponseDTO
        {
            EmployeeId = e.EmployeeId,
            FirstName = e.FirstName,
            MiddleName = e.MiddleName,
            LastName = e.LastName,
            FatherName = e.FatherName,
            MotherName = e.MotherName,
            Gender = e.Gender,
            GenderTitle = codeValuesLookup.GetValueOrDefault(e.Gender, string.Empty),
            DOB = e.DOB,
            AadharNo = e.AadharNo,
            PanNo = e.PanNo,
            EmailId = e.EmailId,
            MobileNo = e.MobileNo,
            PermanentAddress = e.PermanentAddress,
            PAddressCity = e.PAddressCity,
            PAddressState = e.PAddressState,
            PAddressStateName = codeValuesLookup.GetValueOrDefault(e.PAddressState, string.Empty),
            PAddressPincode = e.PAddressPincode,
            CorrespondenceAddress = e.CorrespondenceAddress,
            CAddressCity = e.CAddressCity,
            CAddressState = e.CAddressState,
            CAddressStateName = codeValuesLookup.GetValueOrDefault(e.CAddressState, string.Empty),
            CAddressPincode = e.CAddressPincode,
            Category = e.Category,
            CategoryTitle = codeValuesLookup.GetValueOrDefault(e.Category, string.Empty),
            SubCategory = e.SubCategory,
            SubCategoryTitle = codeValuesLookup.GetValueOrDefault(e.SubCategory, string.Empty),
            SelectionCategory = e.SelectionCategory,
            SelectionCategoryTitle = codeValuesLookup.GetValueOrDefault(e.SelectionCategory, string.Empty),
            HighestQualification = e.HighestQualification,
            HighestQualificationTitle = codeValuesLookup.GetValueOrDefault(e.HighestQualification, string.Empty),
            MaritalStatus = e.MaritalStatus,
            MaritalStatusTitle = codeValuesLookup.GetValueOrDefault(e.MaritalStatus, string.Empty),
            IsAnyDisability = e.IsAnyDisability,
            DisabilityType = e.DisabilityType,
            DisabilityTypeDesc = codeValuesLookup.GetValueOrDefault(e.DisabilityType, string.Empty),
            OtherDisabilityType = e.OtherDisabilityType,
            IsGazetted = e.IsGazetted,
            VehicleFacilityAvailed = e.VehicleFacilityAvailed,
            ReportingPersonId = e.ReportingPersonId,
            DesignationId = e.DesignationId,
            DesignationTitle = e.DesignationTitle,
            DesignationGroupTitle = codeValuesLookup.GetValueOrDefault(e.DesignationGroup, string.Empty),
            IsActive = e.IsActive,
            Remarks = e.Remarks,
            Photo = e.Photo,
            Extension = e.Extension,
            ContentType = e.ContentType
        }).ToList();

        return returnData;

        #region Commented

        //var spParamList = new ParamHelper
        //    {
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_gender", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.Gender },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_category", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.Category },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_selectioncategory", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.SelectionCategory },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_maritalstatus", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.MaritalStatus },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_disabilitytype", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.DisabilityType },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_designationgroup", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.DesignationGroup },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_designationid", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.DesignationId },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_gazetted", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.GazettedOnly.HasValue ?  },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_vehiclefacilityavailed", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.VehiclefacilityAvailed },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isactive", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.Status }                
        //    };

        //ErrorModel error = null;
        //return await ExecuteSPReader(@"select * from spemp_searchemployees(
        //                                                :p_gender,
        //                                                :p_category,
        //                                                :p_selectioncategory,
        //                                                :p_maritalstatus,
        //                                                :p_disabilitytype,
        //                                                :p_designationgroup,
        //                                                :p_designationid,
        //                                                :p_gazetted,
        //                                                :p_vehiclefacilityavailed,
        //                                                :p_isactive                                                       
        //                                                )", spParamList, EmployeeMapper.ToSearchResult, error);

        #endregion
    }

    #endregion

    #region Employee Profile Image

    public async Task<bool> EditPhoto(ProfilePhoto photo, string UpdatedBy)
    {
        var rowsAffected = await dBContext.Employees.Where(x => x.EmployeeId == photo.EmployeeID)
            .ExecuteUpdateAsync(b => b
                .SetProperty(prop => prop.Photo, photo.Photo)
                .SetProperty(prop => prop.Extension, photo.Extension)
                .SetProperty(prop => prop.ContentType, photo.ContentType)
                .SetProperty(prop => prop.ModifiedBy, UpdatedBy)
                .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
            );
        return rowsAffected > 0;

        //var spParamList = new ParamHelper
        //    {
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_employeeid", DBType= NpgsqlDbType.Varchar, ParamValue = photo.EmployeeID},
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_photo", DBType= NpgsqlDbType.Bytea, ParamValue = photo.Photo},
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_extension", DBType= NpgsqlDbType.Varchar, ParamValue = photo.Extension},
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_contenttype", DBType= NpgsqlDbType.Varchar, ParamValue = photo.ContentType}
        //    };

        //ErrorModel? error = null;
        //return await ExecStoredProcedureWithTrans(@"call spemp_updateprofileimage(
        //                                                            :p_employeeid,
        //                                                            :p_photo,
        //                                                            :p_extension,
        //                                                            :p_contenttype)", spParamList, error);
    }

    #endregion

    #region Activate and DeActivate Employee

    public async Task<bool> DeActivateEmployee(string EmployeeId, string UpdatedBy)
    {
        var rowsAffected = await dBContext.Employees.Where(x => x.EmployeeId == EmployeeId).ExecuteUpdateAsync(b => b
            .SetProperty(prop => prop.IsActive, false)
            .SetProperty(prop => prop.ModifiedBy, UpdatedBy)
            .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
        );
        if (rowsAffected > 0)
        {
            await dBContext.Users.Where(x => x.UniqueId == EmployeeId).ExecuteUpdateAsync(b => b
                .SetProperty(prop => prop.IsValid, false)
                .SetProperty(prop => prop.ModifiedBy, UpdatedBy)
                .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
            );
        }

        return rowsAffected > 0 ? true : false;
    }

    public async Task<bool> ActivateEmployee(string EmployeeId, string UpdatedBy)
    {
        var rowsAffected = await dBContext.Employees.Where(x => x.EmployeeId == EmployeeId).ExecuteUpdateAsync(b => b
            .SetProperty(prop => prop.IsActive, true)
            .SetProperty(prop => prop.ModifiedBy, UpdatedBy)
            .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
        );

        if (rowsAffected > 0)
        {
            await dBContext.Users.Where(x => x.UniqueId == EmployeeId).ExecuteUpdateAsync(b => b
                .SetProperty(prop => prop.IsValid, true)
                .SetProperty(prop => prop.ModifiedBy, UpdatedBy)
                .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
            );
        }

        return rowsAffected > 0 ? true : false;
    }

    public async Task<List<EmployeeBasicDto>> GetEmployeesByBranch(string branch) => await (
        from a in dBContext.Employees
        join b in dBContext.EmployeeAppointmentDetails on a.EmployeeId equals b.EmployeeId
        join c in dBContext.Designations on b.Designation equals c.RowId
        where branch == b.CurrentBranch
        select new EmployeeBasicDto
        {
            EmployeeId = a.EmployeeId,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Gender = a.Gender,
            Email = a.EmailId,
            Designation = b.Designation,
            DesignationTitle = c.Title,
        }
    ).AsNoTracking().ToListAsync();

    #endregion
}
