using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialChef.Domain.Relational.Migrations
{
    public partial class Add_RecipeSummary_StepCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StepCount",
                table: "RecipeSummaries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StepCount",
                table: "RecipeSummaries");
        }
    }
}
