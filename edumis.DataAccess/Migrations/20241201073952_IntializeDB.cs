using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IntializeDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberDeviceTokensModel",
                columns: table => new
                {
                    memberid = table.Column<Guid>(type: "uuid", nullable: false),
                    serialno = table.Column<int>(type: "integer", nullable: false),
                    devicename = table.Column<string>(type: "varchar(250)", nullable: false),
                    macaddress = table.Column<string>(type: "varchar(100)", nullable: true),
                    token = table.Column<string>(type: "text", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberDeviceTokensModel", x => new { x.memberid, x.serialno });
                });

            migrationBuilder.CreateTable(
                name: "tbglacademicsessions",
                columns: table => new
                {
                    forsession = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    iscurrent = table.Column<bool>(type: "boolean", nullable: false),
                    registrationstartdate = table.Column<DateOnly>(type: "date", nullable: true),
                    registrationenddate = table.Column<DateOnly>(type: "date", nullable: true),
                    lateregistrationstartdate = table.Column<DateOnly>(type: "date", nullable: true),
                    lateregistrationenddate = table.Column<DateOnly>(type: "date", nullable: true),
                    registrationendtime = table.Column<TimeOnly>(type: "time", nullable: true),
                    lateregistrationendtime = table.Column<TimeOnly>(type: "time", nullable: true),
                    reg_ageasondate = table.Column<DateOnly>(type: "date", nullable: true),
                    registrationstarttime = table.Column<TimeOnly>(type: "time", nullable: true),
                    lateregistrationstarttime = table.Column<TimeOnly>(type: "time", nullable: true),
                    isregistrationopen = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbglacademicsessions", x => x.forsession);
                });

            migrationBuilder.CreateTable(
                name: "tbglcodes",
                columns: table => new
                {
                    code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codedescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbglcodes", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "tbgldesignationmenuitems",
                columns: table => new
                {
                    designationid = table.Column<int>(type: "integer", nullable: false),
                    menuid = table.Column<int>(type: "integer", nullable: false),
                    canview = table.Column<bool>(type: "boolean", nullable: true),
                    cancreate = table.Column<bool>(type: "boolean", nullable: true),
                    canedit = table.Column<bool>(type: "boolean", nullable: true),
                    candelete = table.Column<bool>(type: "boolean", nullable: true),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbgldesignationmenuitems", x => new { x.designationid, x.menuid });
                });

            migrationBuilder.CreateTable(
                name: "tbglexceptionlogs",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    origin = table.Column<string>(type: "varchar(1000)", nullable: true),
                    errormessage = table.Column<string>(type: "varchar(4000)", nullable: true),
                    stacktrace = table.Column<string>(type: "varchar(4000)", nullable: true),
                    innermessage = table.Column<string>(type: "varchar(4000)", nullable: false),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbglexceptionlogs", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbglmapdesigwithusertype",
                columns: table => new
                {
                    designationid = table.Column<int>(type: "integer", nullable: false),
                    usertype = table.Column<int>(type: "integer", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbglmapdesigwithusertype", x => new { x.designationid, x.usertype });
                });

            migrationBuilder.CreateTable(
                name: "tbglmenus",
                columns: table => new
                {
                    menuid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    menutitle = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    parentmenuid = table.Column<int>(type: "integer", nullable: true),
                    module = table.Column<int>(type: "integer", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    menuurl = table.Column<string>(type: "varchar(500)", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbglmenus", x => x.menuid);
                });

            migrationBuilder.CreateTable(
                name: "tbms_districts",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(100)", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbms_districts", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbms_zones",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    districtid = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "varchar(100)", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbms_zones", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbmsbranches",
                columns: table => new
                {
                    branchid = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    buildingid = table.Column<string>(type: "varchar(50)", nullable: false),
                    branchname = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    branchtype = table.Column<int>(type: "integer", nullable: false),
                    parentbranchid = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    zoneid = table.Column<string>(type: "varchar(10)", nullable: true),
                    districtid = table.Column<string>(type: "varchar(10)", nullable: true),
                    inchargeid = table.Column<string>(type: "varchar(50)", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmsbranches", x => x.branchid);
                });

            migrationBuilder.CreateTable(
                name: "tbmsdesignations",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    designationgroup = table.Column<int>(type: "integer", nullable: false),
                    isgazetted = table.Column<bool>(type: "boolean", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmsdesignations", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbmsempappointmentdetails",
                columns: table => new
                {
                    employeeid = table.Column<string>(type: "varchar(50)", nullable: false),
                    designation = table.Column<int>(type: "integer", nullable: false),
                    seniorityno = table.Column<int>(type: "integer", nullable: true),
                    appointmenttype = table.Column<int>(type: "integer", nullable: false),
                    appointmentorder = table.Column<string>(type: "varchar(200)", nullable: true),
                    appointmentdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    branchjoiningdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    recruitmenttype = table.Column<int>(type: "integer", nullable: false),
                    currentpostheld = table.Column<int>(type: "integer", nullable: false),
                    currentbranch = table.Column<string>(type: "varchar(50)", nullable: false),
                    cadre = table.Column<int>(type: "integer", nullable: false),
                    currentscale = table.Column<string>(type: "varchar(50)", nullable: false),
                    grade = table.Column<string>(type: "varchar(50)", nullable: true),
                    gradegrantdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    retirementdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmsempappointmentdetails", x => x.employeeid);
                });

            migrationBuilder.CreateTable(
                name: "tbmsempeducationdetails",
                columns: table => new
                {
                    employeeid = table.Column<string>(type: "varchar(50)", nullable: false),
                    serialno = table.Column<int>(type: "integer", nullable: true),
                    qualification = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    issuedate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    board = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    percentage = table.Column<decimal>(type: "numeric", nullable: true),
                    grade = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    subjects = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmsempeducationdetails", x => x.employeeid);
                });

            migrationBuilder.CreateTable(
                name: "tbmsemployees",
                columns: table => new
                {
                    employeeid = table.Column<string>(type: "varchar(50)", nullable: false),
                    firstname = table.Column<string>(type: "varchar(250)", maxLength: 150, nullable: false),
                    middlename = table.Column<string>(type: "varchar(250)", maxLength: 150, nullable: true),
                    lastname = table.Column<string>(type: "varchar(250)", maxLength: 150, nullable: true),
                    fathername = table.Column<string>(type: "varchar(250)", maxLength: 150, nullable: true),
                    mothername = table.Column<string>(type: "varchar(250)", maxLength: 150, nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: true),
                    dob = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    aadharno = table.Column<string>(type: "varchar(20)", nullable: true),
                    panno = table.Column<string>(type: "varchar(20)", nullable: true),
                    emailid = table.Column<string>(type: "varchar(250)", maxLength: 150, nullable: false),
                    mobileno = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    permanentaddress = table.Column<string>(type: "varchar(500)", maxLength: 250, nullable: false),
                    pcity = table.Column<string>(type: "varchar(150)", nullable: false),
                    pstate = table.Column<int>(type: "integer", nullable: false),
                    ppincode = table.Column<string>(type: "varchar(10)", nullable: false),
                    correspondenceaddress = table.Column<string>(type: "varchar(500)", maxLength: 250, nullable: false),
                    ccity = table.Column<string>(type: "varchar(150)", nullable: false),
                    cstate = table.Column<int>(type: "integer", nullable: false),
                    cpincode = table.Column<string>(type: "varchar(10)", nullable: false),
                    category = table.Column<int>(type: "integer", nullable: true),
                    subcategory = table.Column<int>(type: "integer", nullable: true),
                    selectioncategory = table.Column<int>(type: "integer", nullable: true),
                    highestqualification = table.Column<int>(type: "integer", nullable: true),
                    maritalstatus = table.Column<int>(type: "integer", nullable: true),
                    isanydisability = table.Column<bool>(type: "boolean", nullable: true),
                    disabilitytype = table.Column<int>(type: "integer", nullable: true),
                    otherdisabilitytype = table.Column<string>(type: "varchar(150)", nullable: true),
                    isgazetted = table.Column<bool>(type: "boolean", nullable: true),
                    vehiclefacilityavailed = table.Column<bool>(type: "boolean", nullable: true),
                    reportingpersonid = table.Column<string>(type: "varchar(50)", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    remarks = table.Column<string>(type: "varchar(500)", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmsemployees", x => x.employeeid);
                });

            migrationBuilder.CreateTable(
                name: "tbmsinfrastructure",
                columns: table => new
                {
                    buildingid = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    buildingname = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    location = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    longitude = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    latitude = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    landowning = table.Column<int>(type: "integer", nullable: false),
                    totalfloors = table.Column<int>(type: "integer", nullable: true),
                    totalarea = table.Column<int>(type: "integer", nullable: true),
                    fencing = table.Column<bool>(type: "boolean", nullable: true),
                    tinshed = table.Column<bool>(type: "boolean", nullable: true),
                    park = table.Column<bool>(type: "boolean", nullable: true),
                    totaltrees = table.Column<int>(type: "integer", nullable: true),
                    waterharvesting = table.Column<bool>(type: "boolean", nullable: true),
                    drinkingwater = table.Column<bool>(type: "boolean", nullable: true),
                    toiletfacility = table.Column<bool>(type: "boolean", nullable: true),
                    handicapramp = table.Column<bool>(type: "boolean", nullable: true),
                    cyclestand = table.Column<bool>(type: "boolean", nullable: true),
                    vehicleparking = table.Column<bool>(type: "boolean", nullable: true),
                    accommodation = table.Column<bool>(type: "boolean", nullable: true),
                    badmintoncourt = table.Column<bool>(type: "boolean", nullable: true),
                    tthall = table.Column<bool>(type: "boolean", nullable: true),
                    basketballcourt = table.Column<bool>(type: "boolean", nullable: true),
                    shootingrange = table.Column<bool>(type: "boolean", nullable: true),
                    swimmingpool = table.Column<bool>(type: "boolean", nullable: true),
                    boxingarena = table.Column<bool>(type: "boolean", nullable: true),
                    wrestlingarena = table.Column<bool>(type: "boolean", nullable: true),
                    runningtrack = table.Column<bool>(type: "boolean", nullable: true),
                    weightliftinghall = table.Column<bool>(type: "boolean", nullable: true),
                    lawnteniscourt = table.Column<bool>(type: "boolean", nullable: true),
                    archeryground = table.Column<bool>(type: "boolean", nullable: true),
                    openingyear = table.Column<int>(type: "integer", nullable: true),
                    maintenanceagency = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmsinfrastructure", x => x.buildingid);
                });

            migrationBuilder.CreateTable(
                name: "tbmslogin",
                columns: table => new
                {
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    uniqueid = table.Column<string>(type: "varchar(50)", nullable: false),
                    usertype = table.Column<int>(type: "integer", nullable: false),
                    userrole = table.Column<int>(type: "integer", nullable: false),
                    password = table.Column<string>(type: "varchar(150)", nullable: false),
                    prevpassword1 = table.Column<string>(type: "varchar(150)", nullable: true),
                    prevpassword2 = table.Column<string>(type: "varchar(150)", nullable: true),
                    lastpwdchangeddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ispwdchangewarningsent = table.Column<bool>(type: "boolean", nullable: true),
                    maxnoofinvalidloginattempt = table.Column<int>(type: "integer", nullable: true),
                    isaccountlocked = table.Column<bool>(type: "boolean", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    isloggedin = table.Column<bool>(type: "boolean", nullable: true),
                    photo = table.Column<string>(type: "varchar(250)", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmslogin", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "tbmsnews",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    photo = table.Column<string>(type: "varchar(500)", nullable: true),
                    videolink = table.Column<string>(type: "varchar(500)", nullable: true),
                    externallink = table.Column<string>(type: "varchar(500)", nullable: true),
                    newsdate = table.Column<DateOnly>(type: "date", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmsnews", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbmsposts",
                columns: table => new
                {
                    postcode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    posttitle = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    isgazetted = table.Column<bool>(type: "boolean", nullable: false),
                    orderno = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    orderdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbmsposts", x => x.postcode);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_logins",
                columns: table => new
                {
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    uniqueid = table.Column<string>(type: "varchar(50)", nullable: false),
                    branchid = table.Column<string>(type: "varchar(50)", nullable: false),
                    usertype = table.Column<int>(type: "integer", nullable: false),
                    password = table.Column<string>(type: "varchar(150)", nullable: true),
                    prevpassword1 = table.Column<string>(type: "varchar(150)", nullable: true),
                    prevpassword2 = table.Column<string>(type: "varchar(150)", nullable: true),
                    lastpwdchangeddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ispwdchangewarningsent = table.Column<bool>(type: "boolean", nullable: true),
                    maxnoofinvalidloginattempt = table.Column<int>(type: "integer", nullable: true),
                    isaccountlocked = table.Column<bool>(type: "boolean", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    isloggedin = table.Column<bool>(type: "boolean", nullable: true),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_logins", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_meeting_hist",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    meetingid = table.Column<Guid>(type: "uuid", nullable: false),
                    fieldname = table.Column<string>(type: "varchar(200)", nullable: false),
                    amendmentno = table.Column<int>(type: "integer", nullable: false),
                    fieldvalue = table.Column<string>(type: "text", nullable: false),
                    createdby = table.Column<string>(type: "varchar(200)", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_meeting_hist", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_trans_attachments",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transid = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "varchar(500)", nullable: false),
                    filename = table.Column<string>(type: "varchar(500)", nullable: false),
                    contenttype = table.Column<string>(type: "varchar(100)", nullable: false),
                    extension = table.Column<string>(type: "varchar(50)", nullable: false),
                    filepath = table.Column<string>(type: "varchar(500)", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_trans_attachments", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbweb_circulars",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    financialyear = table.Column<string>(type: "varchar(10)", nullable: false),
                    circulardate = table.Column<DateTime>(type: "date", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    filelink = table.Column<string>(type: "varchar(500)", nullable: true),
                    weblink = table.Column<string>(type: "varchar(500)", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbweb_circulars", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbweb_tenders",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    financialyear = table.Column<string>(type: "varchar(10)", nullable: false),
                    tenderdate = table.Column<DateTime>(type: "date", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    isvalid = table.Column<bool>(type: "boolean", nullable: false),
                    filelink = table.Column<string>(type: "varchar(500)", nullable: true),
                    expirydate = table.Column<DateTime>(type: "date", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbweb_tenders", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "tbglcodevalues",
                columns: table => new
                {
                    codevalue = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codevaldescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    parentcode = table.Column<int>(type: "integer", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    code = table.Column<int>(type: "integer", nullable: false),
                    rowid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbglcodevalues", x => x.codevalue);
                    table.ForeignKey(
                        name: "FK_tbglcodevalues_tbglcodes_code",
                        column: x => x.code,
                        principalTable: "tbglcodes",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_meeting",
                columns: table => new
                {
                    meetingid = table.Column<Guid>(type: "uuid", nullable: false),
                    forsession = table.Column<string>(type: "varchar(10)", nullable: false),
                    branchid = table.Column<string>(type: "varchar(50)", nullable: false),
                    meetingdate = table.Column<DateOnly>(type: "date", nullable: false),
                    meetingtime = table.Column<TimeOnly>(type: "time", nullable: false),
                    title = table.Column<string>(type: "varchar(500)", nullable: false),
                    invitees = table.Column<string[]>(type: "varchar[]", nullable: true),
                    attendees = table.Column<string[]>(type: "varchar[]", nullable: true),
                    mom_brief = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_meeting", x => x.meetingid);
                    table.ForeignKey(
                        name: "FK_tbsmc_meeting_tbglacademicsessions_forsession",
                        column: x => x.forsession,
                        principalTable: "tbglacademicsessions",
                        principalColumn: "forsession",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbsmc_meeting_tbmsbranches_branchid",
                        column: x => x.branchid,
                        principalTable: "tbmsbranches",
                        principalColumn: "branchid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_registeredmembers",
                columns: table => new
                {
                    forsession = table.Column<string>(type: "varchar(10)", nullable: false),
                    uniqueid = table.Column<string>(type: "varchar(50)", nullable: false),
                    branchid = table.Column<string>(type: "varchar(50)", nullable: false),
                    memberid = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(250)", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    designationid = table.Column<int>(type: "integer", nullable: false),
                    membertype = table.Column<int>(type: "integer", nullable: false),
                    mobileno = table.Column<string>(type: "varchar(10)", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: false),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_registeredmembers", x => new { x.forsession, x.branchid, x.uniqueid });
                    table.ForeignKey(
                        name: "FK_tbsmc_registeredmembers_tbglacademicsessions_forsession",
                        column: x => x.forsession,
                        principalTable: "tbglacademicsessions",
                        principalColumn: "forsession",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbsmc_registeredmembers_tbmsbranches_branchid",
                        column: x => x.branchid,
                        principalTable: "tbmsbranches",
                        principalColumn: "branchid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_meeting_agenda",
                columns: table => new
                {
                    meetingid = table.Column<Guid>(type: "uuid", nullable: false),
                    serialno = table.Column<int>(type: "integer", nullable: false),
                    agendacode = table.Column<int>(type: "integer", nullable: false),
                    otherdetails = table.Column<string>(type: "varchar(500)", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_meeting_agenda", x => new { x.meetingid, x.serialno });
                    table.ForeignKey(
                        name: "FK_tbsmc_meeting_agenda_tbsmc_meeting_meetingid",
                        column: x => x.meetingid,
                        principalTable: "tbsmc_meeting",
                        principalColumn: "meetingid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_meeting_attachments",
                columns: table => new
                {
                    meetingid = table.Column<Guid>(type: "uuid", nullable: false),
                    serialno = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "varchar(500)", nullable: true),
                    filename = table.Column<string>(type: "varchar(500)", nullable: true),
                    contenttype = table.Column<string>(type: "varchar(100)", nullable: true),
                    extension = table.Column<string>(type: "varchar(50)", nullable: true),
                    filepath = table.Column<string>(type: "varchar(500)", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_meeting_attachments", x => new { x.meetingid, x.serialno });
                    table.ForeignKey(
                        name: "FK_tbsmc_meeting_attachments_tbsmc_meeting_meetingid",
                        column: x => x.meetingid,
                        principalTable: "tbsmc_meeting",
                        principalColumn: "meetingid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_meeting_resolutions",
                columns: table => new
                {
                    resolutionid = table.Column<Guid>(type: "uuid", nullable: false),
                    meetingid = table.Column<Guid>(type: "uuid", nullable: false),
                    agenda_srno = table.Column<int[]>(type: "integer[]", nullable: true),
                    resolution = table.Column<string>(type: "Text", nullable: false),
                    isclosed = table.Column<bool>(type: "boolean", nullable: true),
                    closingdate = table.Column<DateOnly>(type: "date", nullable: true),
                    comments = table.Column<string>(type: "Text", nullable: true),
                    estimatedcost = table.Column<decimal>(type: "numeric", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_meeting_resolutions", x => x.resolutionid);
                    table.ForeignKey(
                        name: "FK_tbsmc_meeting_resolutions_tbsmc_meeting_meetingid",
                        column: x => x.meetingid,
                        principalTable: "tbsmc_meeting",
                        principalColumn: "meetingid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbsmc_transactions",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    resolutionid = table.Column<Guid>(type: "uuid", nullable: false),
                    transdate = table.Column<DateOnly>(type: "date", nullable: false),
                    transtype = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    refdocno = table.Column<string>(type: "varchar(150)", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbsmc_transactions", x => x.rowid);
                    table.ForeignKey(
                        name: "FK_tbsmc_transactions_tbsmc_meeting_resolutions_resolutionid",
                        column: x => x.resolutionid,
                        principalTable: "tbsmc_meeting_resolutions",
                        principalColumn: "resolutionid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbglcodevalues_code",
                table: "tbglcodevalues",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_meeting_branchid",
                table: "tbsmc_meeting",
                column: "branchid");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_meeting_forsession",
                table: "tbsmc_meeting",
                column: "forsession");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_meeting_resolutions_meetingid",
                table: "tbsmc_meeting_resolutions",
                column: "meetingid");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_registeredmembers_branchid",
                table: "tbsmc_registeredmembers",
                column: "branchid");

            migrationBuilder.CreateIndex(
                name: "IX_tbsmc_transactions_resolutionid",
                table: "tbsmc_transactions",
                column: "resolutionid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberDeviceTokensModel");

            migrationBuilder.DropTable(
                name: "tbglcodevalues");

            migrationBuilder.DropTable(
                name: "tbgldesignationmenuitems");

            migrationBuilder.DropTable(
                name: "tbglexceptionlogs");

            migrationBuilder.DropTable(
                name: "tbglmapdesigwithusertype");

            migrationBuilder.DropTable(
                name: "tbglmenus");

            migrationBuilder.DropTable(
                name: "tbms_districts");

            migrationBuilder.DropTable(
                name: "tbms_zones");

            migrationBuilder.DropTable(
                name: "tbmsdesignations");

            migrationBuilder.DropTable(
                name: "tbmsempappointmentdetails");

            migrationBuilder.DropTable(
                name: "tbmsempeducationdetails");

            migrationBuilder.DropTable(
                name: "tbmsemployees");

            migrationBuilder.DropTable(
                name: "tbmsinfrastructure");

            migrationBuilder.DropTable(
                name: "tbmslogin");

            migrationBuilder.DropTable(
                name: "tbmsnews");

            migrationBuilder.DropTable(
                name: "tbmsposts");

            migrationBuilder.DropTable(
                name: "tbsmc_logins");

            migrationBuilder.DropTable(
                name: "tbsmc_meeting_agenda");

            migrationBuilder.DropTable(
                name: "tbsmc_meeting_attachments");

            migrationBuilder.DropTable(
                name: "tbsmc_meeting_hist");

            migrationBuilder.DropTable(
                name: "tbsmc_registeredmembers");

            migrationBuilder.DropTable(
                name: "tbsmc_trans_attachments");

            migrationBuilder.DropTable(
                name: "tbsmc_transactions");

            migrationBuilder.DropTable(
                name: "tbweb_circulars");

            migrationBuilder.DropTable(
                name: "tbweb_tenders");

            migrationBuilder.DropTable(
                name: "tbglcodes");

            migrationBuilder.DropTable(
                name: "tbsmc_meeting_resolutions");

            migrationBuilder.DropTable(
                name: "tbsmc_meeting");

            migrationBuilder.DropTable(
                name: "tbglacademicsessions");

            migrationBuilder.DropTable(
                name: "tbmsbranches");
        }
    }
}
