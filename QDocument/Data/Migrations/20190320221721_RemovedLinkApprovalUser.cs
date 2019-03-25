using Microsoft.EntityFrameworkCore.Migrations;

namespace QDocument.Data.Migrations
{
    public partial class RemovedLinkApprovalUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentApproval_AspNetUsers_UserId",
                table: "DocumentApproval");

            migrationBuilder.DropIndex(
                name: "IX_DocumentApproval_UserId",
                table: "DocumentApproval");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DocumentApproval");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DocumentApproval",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentApproval_UserId",
                table: "DocumentApproval",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentApproval_AspNetUsers_UserId",
                table: "DocumentApproval",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
