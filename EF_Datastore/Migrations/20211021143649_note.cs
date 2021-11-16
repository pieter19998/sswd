


using Microsoft.EntityFrameworkCore.Migrations;

namespace EF_Datastore.Migrations
{
    public partial class note : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_TreatmentPlans_TreatmentPlanId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_TreatmentPlanId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "TreatmentPlanId",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "Type",

                table: "Sessions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Sessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentPlanId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TreatmentPlanId",
                table: "Notes",
                column: "TreatmentPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_TreatmentPlans_TreatmentPlanId",
                table: "Notes",
                column: "TreatmentPlanId",
                principalTable: "TreatmentPlans",
                principalColumn: "TreatmentPlanId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
