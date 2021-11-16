using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EF_Datastore.Migrations
{
    public partial class session : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreatmentDuration",
                table: "Sessions");

            migrationBuilder.AlterColumn<int>(
                name: "DiagnoseCode",
                table: "TreatmentPlans",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "NoteId",
                table: "TreatmentPlans",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentPlans_Notes_NoteId",
                table: "TreatmentPlans");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentPlans_NoteId",
                table: "TreatmentPlans");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "TreatmentPlans");

            migrationBuilder.AlterColumn<string>(
                name: "DiagnoseCode",
                table: "TreatmentPlans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "TreatmentDuration",
                table: "Sessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
