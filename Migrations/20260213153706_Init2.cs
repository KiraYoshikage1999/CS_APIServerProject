using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CS_APIServerProject.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Characteristics_Color",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Characteristics_DriveType",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Characteristics_Engine",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Characteristics_milege",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Characteristics_state",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Characteristics_typeBody",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Characteristics_typeGas",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Characteristics_typeMilege",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Characteristics",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    state = table.Column<string>(type: "text", nullable: true),
                    typeGas = table.Column<string>(type: "text", nullable: true),
                    milege = table.Column<int>(type: "integer", nullable: false),
                    typeMilege = table.Column<string>(type: "text", nullable: true),
                    typeBody = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    DriveType = table.Column<string>(type: "text", nullable: true),
                    Engine = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Characteristics_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.AddColumn<string>(
                name: "Characteristics_Color",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Characteristics_DriveType",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Characteristics_Engine",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Characteristics_milege",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Characteristics_state",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Characteristics_typeBody",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Characteristics_typeGas",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Characteristics_typeMilege",
                table: "Products",
                type: "text",
                nullable: true);
        }
    }
}
