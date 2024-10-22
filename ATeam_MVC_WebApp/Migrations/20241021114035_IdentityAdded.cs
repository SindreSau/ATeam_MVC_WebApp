using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATeam_MVC_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class IdentityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FoodProducts",
                newName: "FoodProductId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FoodCategories",
                newName: "FoodCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FoodProductId",
                table: "FoodProducts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FoodCategoryId",
                table: "FoodCategories",
                newName: "Id");
        }
    }
}
