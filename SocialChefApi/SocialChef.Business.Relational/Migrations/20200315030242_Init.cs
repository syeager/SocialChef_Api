using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialChef.Business.Relational.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chefs",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chefs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChefRecipes",
                columns: table => new
                {
                    ChefID = table.Column<Guid>(nullable: false),
                    RecipeID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefRecipes", x => new { x.ChefID, x.RecipeID });
                    table.ForeignKey(
                        name: "FK_ChefRecipes_Chefs_ChefID",
                        column: x => x.ChefID,
                        principalTable: "Chefs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChefRecipes");

            migrationBuilder.DropTable(
                name: "Chefs");
        }
    }
}
