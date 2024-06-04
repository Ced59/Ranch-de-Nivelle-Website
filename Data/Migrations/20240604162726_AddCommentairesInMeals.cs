using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RanchDuBonheur.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentairesInMeals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Commentaires",
                table: "Meals",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: false,
                defaultValue: "");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commentaires",
                table: "Meals");

        }
    }
}
