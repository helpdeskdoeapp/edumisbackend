using edumis.Models.Employees.DTO;
using Npgsql;

namespace edumis.DataAccess.Mappers.Employees;

public static class EmployeeMapper
{
    public static List<EmployeeDetailsDTO> ToEmployeeDetails(NpgsqlDataReader reader)
    {
        if (reader == null)
            return null;
        var ReturnModel = new List<EmployeeDetailsDTO>();

        while (reader.Read())
        {
            var DTOModel = new EmployeeDetailsDTO(
                Convert.IsDBNull(reader["employeeid"]) ? string.Empty : reader["employeeid"].ToString(),
                Convert.IsDBNull(reader["firstname"]) ? string.Empty : reader["firstname"].ToString(),
                Convert.IsDBNull(reader["middlename"]) ? string.Empty : reader["middlename"].ToString(),
                Convert.IsDBNull(reader["lastname"]) ? string.Empty : reader["lastname"].ToString(),
                Convert.IsDBNull(reader["fathername"]) ? string.Empty : reader["fathername"].ToString(),
                Convert.IsDBNull(reader["mothername"]) ? string.Empty : reader["mothername"].ToString(),
                Convert.IsDBNull(reader["gender"]) ? 0 : Convert.ToInt32(reader["gender"].ToString()),
                Convert.IsDBNull(reader["gendertitle"]) ? string.Empty : reader["gendertitle"].ToString(),
                Convert.IsDBNull(reader["dob"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["dob"].ToString())),
                Convert.IsDBNull(reader["aadharno"]) ? string.Empty : reader["aadharno"].ToString(),
                Convert.IsDBNull(reader["panno"]) ? string.Empty : reader["panno"].ToString(),
                Convert.IsDBNull(reader["emailid"]) ? string.Empty : reader["emailid"].ToString(),
                Convert.IsDBNull(reader["mobileno"]) ? string.Empty : reader["mobileno"].ToString(),
                Convert.IsDBNull(reader["permanentaddress"]) ? string.Empty : reader["permanentaddress"].ToString(),
                Convert.IsDBNull(reader["pcity"]) ? string.Empty : reader["pcity"].ToString(),
                Convert.IsDBNull(reader["pstate"]) ? 0 : Convert.ToInt32(reader["pstate"].ToString()),
                Convert.IsDBNull(reader["pstatetitle"]) ? string.Empty : reader["pstatetitle"].ToString(),
                Convert.IsDBNull(reader["ppincode"]) ? string.Empty : reader["ppincode"].ToString(),
                Convert.IsDBNull(reader["correspondenceaddress"]) ? string.Empty : reader["correspondenceaddress"].ToString(),
                Convert.IsDBNull(reader["ccity"]) ? string.Empty : reader["ccity"].ToString(),
                Convert.IsDBNull(reader["cstate"]) ? 0 : Convert.ToInt32(reader["cstate"].ToString()),
                Convert.IsDBNull(reader["cstatetitle"]) ? string.Empty : reader["cstatetitle"].ToString(),
                Convert.IsDBNull(reader["cpincode"]) ? string.Empty : reader["cpincode"].ToString(),
                Convert.IsDBNull(reader["category"]) ? 0 : Convert.ToInt32(reader["category"].ToString()),
                Convert.IsDBNull(reader["categorytitle"]) ? string.Empty : reader["categorytitle"].ToString(),
                Convert.IsDBNull(reader["subcategory"]) ? 0 : Convert.ToInt32(reader["subcategory"].ToString()),
                Convert.IsDBNull(reader["subcategorytitle"]) ? string.Empty : reader["subcategorytitle"].ToString(),
                Convert.IsDBNull(reader["selectioncategory"]) ? 0 : Convert.ToInt32(reader["selectioncategory"].ToString()),
                Convert.IsDBNull(reader["selectioncategorytitle"]) ? string.Empty : reader["selectioncategorytitle"].ToString(),
                Convert.IsDBNull(reader["highestqualification"]) ? 0 : Convert.ToInt32(reader["highestqualification"].ToString()),
                Convert.IsDBNull(reader["highestqualificationtitle"]) ? string.Empty : reader["highestqualificationtitle"].ToString(),
                Convert.IsDBNull(reader["maritalstatus"]) ? 0 : Convert.ToInt32(reader["maritalstatus"].ToString()),
                Convert.IsDBNull(reader["maritalstatustitle"]) ? string.Empty : reader["maritalstatustitle"].ToString(),
                Convert.IsDBNull(reader["isanydisability"]) ? false : Convert.ToBoolean(reader["isanydisability"].ToString()),
                Convert.IsDBNull(reader["disabilitytype"]) ? 0 : Convert.ToInt32(reader["disabilitytype"].ToString()),
                Convert.IsDBNull(reader["disabilitytypetitle"]) ? string.Empty : reader["disabilitytypetitle"].ToString(),
                Convert.IsDBNull(reader["otherdisabilitytype"]) ? string.Empty : reader["otherdisabilitytype"].ToString(),
                Convert.IsDBNull(reader["isgazetted"]) ? false : Convert.ToBoolean(reader["isgazetted"].ToString()),
                Convert.IsDBNull(reader["vehiclefacilityavailed"]) ? false : Convert.ToBoolean(reader["vehiclefacilityavailed"].ToString()),
                Convert.IsDBNull(reader["reportingpersonid"]) ? string.Empty : reader["reportingpersonid"].ToString(),
                Convert.IsDBNull(reader["isactive"]) ? false : Convert.ToBoolean(reader["isactive"].ToString()),
                Convert.IsDBNull(reader["remarks"]) ? string.Empty : reader["remarks"].ToString(),
                Convert.IsDBNull(reader["designation"]) ? 0 : Convert.ToInt32(reader["designation"].ToString()),
                Convert.IsDBNull(reader["designationtitle"]) ? string.Empty : reader["designationtitle"].ToString(),
                Convert.IsDBNull(reader["seniorityno"]) ? 0 : Convert.ToInt32(reader["seniorityno"].ToString()),
                Convert.IsDBNull(reader["appointmenttype"]) ? 0 : Convert.ToInt32(reader["appointmenttype"].ToString()),
                Convert.IsDBNull(reader["appointmenttypedesc"]) ? string.Empty : reader["appointmenttypedesc"].ToString(),
                Convert.IsDBNull(reader["appointmentorder"]) ? string.Empty : reader["appointmentorder"].ToString(),
                Convert.IsDBNull(reader["appointmentdate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["appointmentdate"].ToString())),
                Convert.IsDBNull(reader["branchjoiningdate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["branchjoiningdate"].ToString())),
                Convert.IsDBNull(reader["recruitmenttype"]) ? 0 : Convert.ToInt32(reader["recruitmenttype"].ToString()),
                Convert.IsDBNull(reader["recruitmenttypedesc"]) ? string.Empty : reader["recruitmenttypedesc"].ToString(),
                Convert.IsDBNull(reader["currentpostheld"]) ? 0 : Convert.ToInt32(reader["currentpostheld"].ToString()),
                Convert.IsDBNull(reader["currentposttitle"]) ? string.Empty : reader["currentposttitle"].ToString(),
                Convert.IsDBNull(reader["currentbranchid"]) ? string.Empty : reader["currentbranchid"].ToString(),
                Convert.IsDBNull(reader["currentbranchname"]) ? string.Empty : reader["currentbranchname"].ToString(),
                Convert.IsDBNull(reader["cadre"]) ? 0 : Convert.ToInt32(reader["cadre"].ToString()),
                Convert.IsDBNull(reader["cadretitle"]) ? string.Empty : reader["cadretitle"].ToString(),
                Convert.IsDBNull(reader["currentscale"]) ? string.Empty : reader["currentscale"].ToString(),
                Convert.IsDBNull(reader["grade"]) ? string.Empty : reader["grade"].ToString(),
                Convert.IsDBNull(reader["gradegrantdate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["gradegrantdate"].ToString())),
                Convert.IsDBNull(reader["retirementdate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["retirementdate"].ToString())),
                Convert.IsDBNull(reader["photo"]) ? null : (byte[])reader["photo"],
                Convert.IsDBNull(reader["extension"]) ? string.Empty : reader["extension"].ToString(),
                Convert.IsDBNull(reader["contenttype"]) ? string.Empty : reader["contenttype"].ToString(),
                null
            );

            ReturnModel.Add(DTOModel);
        }
        return ReturnModel;
    }

    #region Commented Code
    //public static List<SearchResultDTO> ToSearchResult(NpgsqlDataReader reader)
    //{
    //    if (reader == null)
    //        return null;
    //    var ReturnModel = new List<SearchResultDTO>();

    //    while (reader.Read())
    //    {
    //        var DTOModel = new SearchResultDTO(
    //            Convert.IsDBNull(reader["employeeid"]) ? string.Empty : reader["employeeid"].ToString(),
    //            Convert.IsDBNull(reader["firstname"]) ? string.Empty : reader["firstname"].ToString(),
    //            Convert.IsDBNull(reader["middlename"]) ? string.Empty : reader["middlename"].ToString(),
    //            Convert.IsDBNull(reader["lastname"]) ? string.Empty : reader["lastname"].ToString(),
    //            Convert.IsDBNull(reader["fathername"]) ? string.Empty : reader["fathername"].ToString(),
    //            Convert.IsDBNull(reader["mothername"]) ? string.Empty : reader["mothername"].ToString(),
    //            Convert.IsDBNull(reader["gender"]) ? 0 : Convert.ToInt32(reader["gender"].ToString()),
    //            Convert.IsDBNull(reader["gendertitle"]) ? string.Empty : reader["gendertitle"].ToString(),
    //            Convert.IsDBNull(reader["dob"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["dob"].ToString())),
    //            Convert.IsDBNull(reader["aadharno"]) ? string.Empty : reader["aadharno"].ToString(),
    //            Convert.IsDBNull(reader["panno"]) ? string.Empty : reader["panno"].ToString(),
    //            Convert.IsDBNull(reader["emailid"]) ? string.Empty : reader["emailid"].ToString(),
    //            Convert.IsDBNull(reader["mobileno"]) ? string.Empty : reader["mobileno"].ToString(),
    //            Convert.IsDBNull(reader["permanentaddress"]) ? string.Empty : reader["permanentaddress"].ToString(),
    //            Convert.IsDBNull(reader["pcity"]) ? string.Empty : reader["pcity"].ToString(),
    //            Convert.IsDBNull(reader["pstate"]) ? 0 : Convert.ToInt32(reader["pstate"].ToString()),
    //            Convert.IsDBNull(reader["pstatetitle"]) ? string.Empty : reader["pstatetitle"].ToString(),
    //            Convert.IsDBNull(reader["ppincode"]) ? string.Empty : reader["ppincode"].ToString(),
    //            Convert.IsDBNull(reader["correspondenceaddress"]) ? string.Empty : reader["correspondenceaddress"].ToString(),
    //            Convert.IsDBNull(reader["ccity"]) ? string.Empty : reader["ccity"].ToString(),
    //            Convert.IsDBNull(reader["cstate"]) ? 0 : Convert.ToInt32(reader["cstate"].ToString()),
    //            Convert.IsDBNull(reader["cstatetitle"]) ? string.Empty : reader["cstatetitle"].ToString(),
    //            Convert.IsDBNull(reader["cpincode"]) ? string.Empty : reader["cpincode"].ToString(),
    //            Convert.IsDBNull(reader["category"]) ? 0 : Convert.ToInt32(reader["category"].ToString()),
    //            Convert.IsDBNull(reader["categorytitle"]) ? string.Empty : reader["categorytitle"].ToString(),
    //            Convert.IsDBNull(reader["subcategory"]) ? 0 : Convert.ToInt32(reader["subcategory"].ToString()),
    //            Convert.IsDBNull(reader["subcategorytitle"]) ? string.Empty : reader["subcategorytitle"].ToString(),
    //            Convert.IsDBNull(reader["selectioncategory"]) ? 0 : Convert.ToInt32(reader["selectioncategory"].ToString()),
    //            Convert.IsDBNull(reader["selectioncategorytitle"]) ? string.Empty : reader["selectioncategorytitle"].ToString(),
    //            Convert.IsDBNull(reader["highestqualification"]) ? 0 : Convert.ToInt32(reader["highestqualification"].ToString()),
    //            Convert.IsDBNull(reader["highestqualificationtitle"]) ? string.Empty : reader["highestqualificationtitle"].ToString(),
    //            Convert.IsDBNull(reader["maritalstatus"]) ? 0 : Convert.ToInt32(reader["maritalstatus"].ToString()),
    //            Convert.IsDBNull(reader["maritalstatustitle"]) ? string.Empty : reader["maritalstatustitle"].ToString(),
    //            Convert.IsDBNull(reader["isanydisability"]) ? false : Convert.ToBoolean(reader["isanydisability"].ToString()),
    //            Convert.IsDBNull(reader["disabilitytype"]) ? 0 : Convert.ToInt32(reader["disabilitytype"].ToString()),
    //            Convert.IsDBNull(reader["disabilitytypetitle"]) ? string.Empty : reader["disabilitytypetitle"].ToString(),
    //            Convert.IsDBNull(reader["otherdisabilitytype"]) ? string.Empty : reader["otherdisabilitytype"].ToString(),
    //            Convert.IsDBNull(reader["isgazetted"]) ? false : Convert.ToBoolean(reader["isgazetted"].ToString()),
    //            Convert.IsDBNull(reader["vehiclefacilityavailed"]) ? false : Convert.ToBoolean(reader["vehiclefacilityavailed"].ToString()),
    //            Convert.IsDBNull(reader["reportingpersonid"]) ? string.Empty : reader["reportingpersonid"].ToString(),
    //            Convert.IsDBNull(reader["designationid"]) ? 0 : Convert.ToInt32(reader["designationid"].ToString()),
    //            Convert.IsDBNull(reader["designationtitle"]) ? string.Empty : reader["designationtitle"].ToString(),
    //            Convert.IsDBNull(reader["isactive"]) ? false : Convert.ToBoolean(reader["isactive"].ToString()),
    //            Convert.IsDBNull(reader["remarks"]) ? string.Empty : reader["remarks"].ToString(),
    //            Convert.IsDBNull(reader["photo"]) ? null : (byte[])reader["photo"],
    //            Convert.IsDBNull(reader["extension"]) ? string.Empty : reader["extension"].ToString(),
    //            Convert.IsDBNull(reader["contenttype"]) ? string.Empty : reader["contenttype"].ToString()
    //        );

    //        ReturnModel.Add(DTOModel);
    //    }
    //    return ReturnModel;
    //}
    #endregion

    public static List<AppointmentDetailsDTO> ToAppointmentDetails(NpgsqlDataReader reader)
    {
        if (reader == null)
            return null;
        var ReturnModel = new List<AppointmentDetailsDTO>();

        while (reader.Read())
        {
            var DTOModel = new AppointmentDetailsDTO(
                Convert.IsDBNull(reader["employeeid"]) ? string.Empty : reader["employeeid"].ToString(),
                Convert.IsDBNull(reader["designation"]) ? 0 : Convert.ToInt32(reader["designation"].ToString()),
                Convert.IsDBNull(reader["designationtitle"]) ? string.Empty : reader["designationtitle"].ToString(),
                Convert.IsDBNull(reader["seniorityno"]) ? 0 : Convert.ToInt32(reader["seniorityno"].ToString()),
                Convert.IsDBNull(reader["appointmenttype"]) ? 0 : Convert.ToInt32(reader["appointmenttype"].ToString()),
                Convert.IsDBNull(reader["appointmenttypedesc"]) ? string.Empty : reader["appointmenttypedesc"].ToString(),
                Convert.IsDBNull(reader["appointmentorder"]) ? string.Empty : reader["appointmentorder"].ToString(),
                Convert.IsDBNull(reader["appointmentdate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["appointmentdate"].ToString())),
                Convert.IsDBNull(reader["branchjoiningdate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["branchjoiningdate"].ToString())),
                Convert.IsDBNull(reader["recruitmenttype"]) ? 0 : Convert.ToInt32(reader["recruitmenttype"].ToString()),
                Convert.IsDBNull(reader["recruitmenttypedesc"]) ? string.Empty : reader["recruitmenttypedesc"].ToString(),
                Convert.IsDBNull(reader["selectioncategory"]) ? 0 : Convert.ToInt32(reader["selectioncategory"].ToString()),
                Convert.IsDBNull(reader["selectioncategorytitle"]) ? string.Empty : reader["selectioncategorytitle"].ToString(),
                Convert.IsDBNull(reader["currentpostheld"]) ? 0 : Convert.ToInt32(reader["currentpostheld"].ToString()),
                Convert.IsDBNull(reader["currentposttitle"]) ? string.Empty : reader["currentposttitle"].ToString(),
                Convert.IsDBNull(reader["currentbranchid"]) ? string.Empty : reader["currentbranchid"].ToString(),
                Convert.IsDBNull(reader["currentbranchname"]) ? string.Empty : reader["currentbranchname"].ToString(),
                Convert.IsDBNull(reader["cadre"]) ? 0 : Convert.ToInt32(reader["cadre"].ToString()),
                Convert.IsDBNull(reader["cadretitle"]) ? string.Empty : reader["cadretitle"].ToString(),
                Convert.IsDBNull(reader["currentscale"]) ? string.Empty : reader["currentscale"].ToString(),
                Convert.IsDBNull(reader["grade"]) ? string.Empty : reader["grade"].ToString(),
                Convert.IsDBNull(reader["gradegrantdate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["gradegrantdate"].ToString())),
                Convert.IsDBNull(reader["retirementdate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["retirementdate"].ToString()))
            );

            ReturnModel.Add(DTOModel);
        }
        return ReturnModel;
    }

    public static List<EducationDetailsDTO> ToEducationalDetails(NpgsqlDataReader reader)
    {
        if (reader == null)
            return null;
        var ReturnModel = new List<EducationDetailsDTO>();

        while (reader.Read())
        {
            var DTOModel = new EducationDetailsDTO(
                Convert.IsDBNull(reader["recordid"]) ? 0 : Convert.ToInt64(reader["recordid"].ToString()),
                Convert.IsDBNull(reader["employeeid"]) ? string.Empty : reader["employeeid"].ToString(),
                Convert.IsDBNull(reader["serialno"]) ? 0 : Convert.ToInt32(reader["serialno"].ToString()),
                Convert.IsDBNull(reader["qualification"]) ? 0 : Convert.ToInt32(reader["qualification"].ToString()),
                Convert.IsDBNull(reader["qualificationtitle"]) ? string.Empty : reader["qualificationtitle"].ToString(),
                Convert.IsDBNull(reader["title"]) ? string.Empty : reader["title"].ToString(),
                Convert.IsDBNull(reader["issuedate"]) ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : DateOnly.FromDateTime(Convert.ToDateTime(reader["issuedate"].ToString())),
                Convert.IsDBNull(reader["board"]) ? string.Empty : reader["board"].ToString(),
                Convert.IsDBNull(reader["percentage"]) ? 0 : Convert.ToDecimal(reader["percentage"].ToString()),
                Convert.IsDBNull(reader["grade"]) ? string.Empty : reader["grade"].ToString(),
                Convert.IsDBNull(reader["subjects"]) ? string.Empty : reader["subjects"].ToString()
            );
            ReturnModel.Add(DTOModel);
        }
        return ReturnModel;
    }
}
