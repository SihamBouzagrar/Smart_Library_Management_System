using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Library_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowItems_Books_BookID",
                table: "BorrowItems");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowItems_Books_BookID",
                table: "BorrowItems",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowItems_Books_BookID",
                table: "BorrowItems");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowItems_Books_BookID",
                table: "BorrowItems",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
