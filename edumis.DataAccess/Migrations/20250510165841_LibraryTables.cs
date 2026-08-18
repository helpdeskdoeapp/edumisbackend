using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LibraryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbbk_bookdetails",
                columns: table => new
                {
                    bookid = table.Column<Guid>(type: "uuid", nullable: false),
                    booklevel = table.Column<int>(type: "integer", nullable: false),
                    booktype = table.Column<int>(type: "integer", nullable: false),
                    volumeno = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "varchar(250)", nullable: false),
                    subtitle = table.Column<string>(type: "varchar(250)", nullable: false),
                    author_first_name = table.Column<string>(type: "varchar(150)", nullable: true),
                    author_mid_name = table.Column<string>(type: "varchar(150)", nullable: true),
                    author_last_name = table.Column<string>(type: "varchar(150)", nullable: true),
                    publisher = table.Column<string>(type: "varchar(250)", nullable: true),
                    editor = table.Column<string>(type: "varchar(250)", nullable: true),
                    classcode = table.Column<int>(type: "integer", nullable: true),
                    subject = table.Column<int>(type: "integer", nullable: true),
                    language = table.Column<int>(type: "integer", nullable: false),
                    genre = table.Column<int[]>(type: "integer[]", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    ddcno = table.Column<string>(type: "varchar(150)", nullable: true),
                    subdivisionno = table.Column<string>(type: "varchar(150)", nullable: true),
                    coverimageurl = table.Column<string>(type: "varchar(250)", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string>(type: "varchar(500)", nullable: true),
                    rating = table.Column<int>(type: "integer", nullable: true),
                    ebookurl = table.Column<string>(type: "varchar(250)", nullable: true),
                    audiourl = table.Column<string>(type: "varchar(250)", nullable: true),
                    videourl = table.Column<string>(type: "varchar(250)", nullable: true),
                    relatedbooks = table.Column<Guid[]>(type: "uuid[]", nullable: true),
                    awards = table.Column<string>(type: "text", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbk_bookdetails", x => x.bookid);
                });

            migrationBuilder.CreateTable(
                name: "tbbk_magazine",
                columns: table => new
                {
                    magazineid = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(250)", nullable: false),
                    publisher = table.Column<string>(type: "varchar(250)", nullable: true),
                    editor = table.Column<string>(type: "varchar(250)", nullable: true),
                    language = table.Column<int>(type: "integer", nullable: false),
                    genre = table.Column<int[]>(type: "integer[]", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    coverimageurl = table.Column<string>(type: "varchar(250)", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string>(type: "varchar(500)", nullable: true),
                    rating = table.Column<int>(type: "integer", nullable: true),
                    ebookurl = table.Column<string>(type: "varchar(250)", nullable: true),
                    audiourl = table.Column<string>(type: "varchar(250)", nullable: true),
                    videourl = table.Column<string>(type: "varchar(250)", nullable: true),
                    related_magazines = table.Column<Guid[]>(type: "uuid[]", nullable: true),
                    awards = table.Column<string>(type: "text", nullable: true),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbk_magazine", x => x.magazineid);
                });

            migrationBuilder.CreateTable(
                name: "tbbk_newspaper",
                columns: table => new
                {
                    newspaperid = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "varchar(250)", nullable: false),
                    language = table.Column<int>(type: "integer", nullable: false),
                    genre = table.Column<int[]>(type: "integer[]", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    ebookurl = table.Column<string>(type: "varchar(250)", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbk_newspaper", x => x.newspaperid);
                });

            migrationBuilder.CreateTable(
                name: "tbbk_catalogue",
                columns: table => new
                {
                    bookid = table.Column<Guid>(type: "uuid", nullable: false),
                    isbn = table.Column<string>(type: "varchar(50)", nullable: true),
                    accessionno = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<string>(type: "varchar(250)", nullable: true),
                    shelf = table.Column<string>(type: "varchar(250)", nullable: true),
                    callno = table.Column<string>(type: "varchar(250)", nullable: true),
                    condition = table.Column<int>(type: "integer", nullable: false),
                    edition = table.Column<string>(type: "varchar(250)", nullable: true),
                    publication_year = table.Column<int>(type: "integer", nullable: true),
                    no_of_pages = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_tbbk_catalogue", x => x.bookid);
                    table.ForeignKey(
                        name: "FK_tbbk_catalogue_tbbk_bookdetails_bookid",
                        column: x => x.bookid,
                        principalTable: "tbbk_bookdetails",
                        principalColumn: "bookid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbbk_procure_trans",
                columns: table => new
                {
                    bookid = table.Column<Guid>(type: "uuid", nullable: false),
                    transid = table.Column<Guid>(type: "uuid", nullable: false),
                    procurement_source = table.Column<int>(type: "integer", nullable: false),
                    procurementdate = table.Column<DateOnly>(type: "date", nullable: true),
                    other_procurement_src = table.Column<string>(type: "varchar(250)", nullable: true),
                    billno = table.Column<string>(type: "varchar(150)", nullable: true),
                    billdate = table.Column<DateOnly>(type: "date", nullable: true),
                    billamount = table.Column<decimal>(type: "numeric", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbk_procure_trans", x => new { x.bookid, x.transid });
                    table.ForeignKey(
                        name: "FK_tbbk_procure_trans_tbbk_bookdetails_bookid",
                        column: x => x.bookid,
                        principalTable: "tbbk_bookdetails",
                        principalColumn: "bookid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbbk_reviews",
                columns: table => new
                {
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bookid = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewtext = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: true),
                    reviewerid = table.Column<string>(type: "varchar(100)", nullable: true),
                    isapproved = table.Column<bool>(type: "boolean", nullable: false),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbk_reviews", x => x.rowid);
                    table.ForeignKey(
                        name: "FK_tbbk_reviews_tbbk_bookdetails_bookid",
                        column: x => x.bookid,
                        principalTable: "tbbk_bookdetails",
                        principalColumn: "bookid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbbk_magazine_procure_trans",
                columns: table => new
                {
                    magazineid = table.Column<Guid>(type: "uuid", nullable: false),
                    transid = table.Column<Guid>(type: "uuid", nullable: false),
                    procurement_source = table.Column<int>(type: "integer", nullable: false),
                    procurementdate = table.Column<DateOnly>(type: "date", nullable: true),
                    other_procurement_src = table.Column<string>(type: "varchar(250)", nullable: true),
                    billno = table.Column<string>(type: "varchar(150)", nullable: true),
                    billdate = table.Column<DateOnly>(type: "date", nullable: true),
                    billamount = table.Column<decimal>(type: "numeric", nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    rowid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    createdby = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    modifiedby = table.Column<string>(type: "text", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbk_magazine_procure_trans", x => new { x.magazineid, x.transid });
                    table.ForeignKey(
                        name: "FK_tbbk_magazine_procure_trans_tbbk_magazine_magazineid",
                        column: x => x.magazineid,
                        principalTable: "tbbk_magazine",
                        principalColumn: "magazineid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbbk_catalogue_accessionno",
                table: "tbbk_catalogue",
                column: "accessionno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbbk_reviews_bookid",
                table: "tbbk_reviews",
                column: "bookid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbbk_catalogue");

            migrationBuilder.DropTable(
                name: "tbbk_magazine_procure_trans");

            migrationBuilder.DropTable(
                name: "tbbk_newspaper");

            migrationBuilder.DropTable(
                name: "tbbk_procure_trans");

            migrationBuilder.DropTable(
                name: "tbbk_reviews");

            migrationBuilder.DropTable(
                name: "tbbk_magazine");

            migrationBuilder.DropTable(
                name: "tbbk_bookdetails");
        }
    }
}
