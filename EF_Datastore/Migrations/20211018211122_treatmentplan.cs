
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF_Datastore.Migrations
{
    public partial class treatmentplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentPlans_Notes_NoteId",
                table: "TreatmentPlans");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentPlans_NoteId",
                table: "TreatmentPlans");

            migrationBuilder.DropColumn(
                name: "DiagnoseCode",
                table: "TreatmentPlans");

            migrationBuilder.DropColumn(
                name: "DiagnoseDescription",
                table: "TreatmentPlans");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "TreatmentPlans");

            migrationBuilder.AlterColumn<string>(
                name: "PatientNumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "NoteType",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TreatmentPlanId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiagnoseDescription",
                table: "Dossiers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Email",
                table: "Patients",
                column: "Email",
                unique: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_TreatmentPlans_TreatmentPlanId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Email",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Notes_TreatmentPlanId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "NoteType",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "TreatmentPlanId",
                table: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "DiagnoseCode",
                table: "TreatmentPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DiagnoseDescription",
                table: "TreatmentPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NoteId",
                table: "TreatmentPlans",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PatientNumber",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiagnoseDescription",
                table: "Dossiers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlans_NoteId",
                table: "TreatmentPlans",
                column: "NoteId",
                unique: true,
                filter: "[NoteId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentPlans_Notes_NoteId",
                table: "TreatmentPlans",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoticeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
