using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class DetailsDBadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusActive",
                table: "Products");

            migrationBuilder.AlterColumn<bool>(
                name: "StatusActived",
                table: "Providers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "StatusActived",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "StatusActived",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusActived",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StatusActived",
                table: "Categories");

            migrationBuilder.AlterColumn<bool>(
                name: "StatusActived",
                table: "Providers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "StatusActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
