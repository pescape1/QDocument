using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QDocument.Data.Migrations
{
    public partial class AddedDocApprovalKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentApproval",
                table: "DocumentApproval");

            migrationBuilder.DropIndex(
                name: "IX_DocumentApproval_DocumentID",
                table: "DocumentApproval");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "DocumentApproval");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentApproval",
                table: "DocumentApproval",
                columns: new[] { "DocumentID", "JobID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DocumentApproval",
                table: "DocumentApproval");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "DocumentApproval",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocumentApproval",
                table: "DocumentApproval",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentApproval_DocumentID",
                table: "DocumentApproval",
                column: "DocumentID");
        }
    }
}
