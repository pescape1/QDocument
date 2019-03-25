using Microsoft.EntityFrameworkCore.Migrations;

namespace QDocument.Data.Migrations
{
    public partial class ChangedDocumentApprovaltoJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobID",
                table: "DocumentApproval",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentApproval_JobID",
                table: "DocumentApproval",
                column: "JobID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentApproval_Job_JobID",
                table: "DocumentApproval",
                column: "JobID",
                principalTable: "Job",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentApproval_Job_JobID",
                table: "DocumentApproval");

            migrationBuilder.DropIndex(
                name: "IX_DocumentApproval_JobID",
                table: "DocumentApproval");

            migrationBuilder.DropColumn(
                name: "JobID",
                table: "DocumentApproval");
        }
    }
}
