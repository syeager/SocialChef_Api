using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialChef.Domain.Relational.Migrations
{
    public partial class Add_RecipeSummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChefRecipes");

            migrationBuilder.CreateTable(
                name: "RecipeSummaries",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    RecipeName = table.Column<string>(nullable: false),
                    ChefId = table.Column<Guid>(nullable: false),
                    ChefDaoID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSummaries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecipeSummaries_Chefs_ChefDaoID",
                        column: x => x.ChefDaoID,
                        principalTable: "Chefs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSummaries_ChefDaoID",
                table: "RecipeSummaries",
                column: "ChefDaoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeSummaries");

            migrationBuilder.CreateTable(
                name: "ChefRecipes",
                columns: table => new
                {
                    ChefID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
    }
}
