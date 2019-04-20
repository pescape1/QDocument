using Microsoft.EntityFrameworkCore.Migrations;

namespace QDocument.Data.Migrations
{
    public partial class AddedUniqueIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Job_ShortTitle",
                table: "Job",
                column: "ShortTitle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Job_Title",
                table: "Job",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Document_Title",
                table: "Document",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Job_ShortTitle",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Job_Title",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Document_Title",
                table: "Document");
        }
    }
}
