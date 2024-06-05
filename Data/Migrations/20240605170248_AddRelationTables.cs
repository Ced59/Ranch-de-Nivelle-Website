using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RanchDuBonheur.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealArtist_Artists_ArtistId",
                table: "MealArtist");

            migrationBuilder.DropForeignKey(
                name: "FK_MealArtist_Meals_MealId",
                table: "MealArtist");

            migrationBuilder.DropForeignKey(
                name: "FK_MealDish_Dishes_DishId",
                table: "MealDish");

            migrationBuilder.DropForeignKey(
                name: "FK_MealDish_Meals_MealId",
                table: "MealDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealDish",
                table: "MealDish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealArtist",
                table: "MealArtist");

            migrationBuilder.RenameTable(
                name: "MealDish",
                newName: "MealDishes");

            migrationBuilder.RenameTable(
                name: "MealArtist",
                newName: "MealArtists");

            migrationBuilder.RenameIndex(
                name: "IX_MealDish_MealId",
                table: "MealDishes",
                newName: "IX_MealDishes_MealId");

            migrationBuilder.RenameIndex(
                name: "IX_MealDish_DishId",
                table: "MealDishes",
                newName: "IX_MealDishes_DishId");

            migrationBuilder.RenameIndex(
                name: "IX_MealArtist_MealId",
                table: "MealArtists",
                newName: "IX_MealArtists_MealId");

            migrationBuilder.RenameIndex(
                name: "IX_MealArtist_ArtistId",
                table: "MealArtists",
                newName: "IX_MealArtists_ArtistId");

            migrationBuilder.AlterColumn<string>(
                name: "Commentaires",
                table: "Meals",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2500)",
                oldMaxLength: 2500);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealDishes",
                table: "MealDishes",
                column: "MailDishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealArtists",
                table: "MealArtists",
                column: "MailArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealArtists_Artists_ArtistId",
                table: "MealArtists",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealArtists_Meals_MealId",
                table: "MealArtists",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealDishes_Dishes_DishId",
                table: "MealDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealDishes_Meals_MealId",
                table: "MealDishes",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealArtists_Artists_ArtistId",
                table: "MealArtists");

            migrationBuilder.DropForeignKey(
                name: "FK_MealArtists_Meals_MealId",
                table: "MealArtists");

            migrationBuilder.DropForeignKey(
                name: "FK_MealDishes_Dishes_DishId",
                table: "MealDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_MealDishes_Meals_MealId",
                table: "MealDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealDishes",
                table: "MealDishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MealArtists",
                table: "MealArtists");

            migrationBuilder.RenameTable(
                name: "MealDishes",
                newName: "MealDish");

            migrationBuilder.RenameTable(
                name: "MealArtists",
                newName: "MealArtist");

            migrationBuilder.RenameIndex(
                name: "IX_MealDishes_MealId",
                table: "MealDish",
                newName: "IX_MealDish_MealId");

            migrationBuilder.RenameIndex(
                name: "IX_MealDishes_DishId",
                table: "MealDish",
                newName: "IX_MealDish_DishId");

            migrationBuilder.RenameIndex(
                name: "IX_MealArtists_MealId",
                table: "MealArtist",
                newName: "IX_MealArtist_MealId");

            migrationBuilder.RenameIndex(
                name: "IX_MealArtists_ArtistId",
                table: "MealArtist",
                newName: "IX_MealArtist_ArtistId");

            migrationBuilder.AlterColumn<string>(
                name: "Commentaires",
                table: "Meals",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2500)",
                oldMaxLength: 2500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealDish",
                table: "MealDish",
                column: "MailDishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MealArtist",
                table: "MealArtist",
                column: "MailArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealArtist_Artists_ArtistId",
                table: "MealArtist",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealArtist_Meals_MealId",
                table: "MealArtist",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealDish_Dishes_DishId",
                table: "MealDish",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealDish_Meals_MealId",
                table: "MealDish",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
