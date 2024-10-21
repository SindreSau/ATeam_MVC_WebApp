using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATeam_MVC_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemovedApplicationUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

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

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");
        }
    }
}
