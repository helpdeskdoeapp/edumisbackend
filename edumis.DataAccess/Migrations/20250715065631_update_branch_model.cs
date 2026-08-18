using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace edumis.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update_branch_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbmsbranches_tbmsinfrastructure_InfrastructureNavigationBui~",
                table: "tbmsbranches");

            migrationBuilder.RenameColumn(
                name: "InfrastructureNavigationBuildingId",
                table: "tbmsbranches",
                newName: "InfrastructureModelBuildingId");

            migrationBuilder.RenameIndex(
                name: "IX_tbmsbranches_InfrastructureNavigationBuildingId",
                table: "tbmsbranches",
                newName: "IX_tbmsbranches_InfrastructureModelBuildingId");

            migrationBuilder.AlterColumn<string>(
                name: "buildingid",
                table: "tbmsbranches",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AddForeignKey(
                name: "FK_tbmsbranches_tbmsinfrastructure_InfrastructureModelBuilding~",
                table: "tbmsbranches",
                column: "InfrastructureModelBuildingId",
                principalTable: "tbmsinfrastructure",
                principalColumn: "buildingid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbmsbranches_tbmsinfrastructure_InfrastructureModelBuilding~",
                table: "tbmsbranches");

            migrationBuilder.RenameColumn(
                name: "InfrastructureModelBuildingId",
                table: "tbmsbranches",
                newName: "InfrastructureNavigationBuildingId");

            migrationBuilder.RenameIndex(
                name: "IX_tbmsbranches_InfrastructureModelBuildingId",
                table: "tbmsbranches",
                newName: "IX_tbmsbranches_InfrastructureNavigationBuildingId");

            migrationBuilder.AlterColumn<string>(
                name: "buildingid",
                table: "tbmsbranches",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbmsbranches_tbmsinfrastructure_InfrastructureNavigationBui~",
                table: "tbmsbranches",
                column: "InfrastructureNavigationBuildingId",
                principalTable: "tbmsinfrastructure",
                principalColumn: "buildingid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
