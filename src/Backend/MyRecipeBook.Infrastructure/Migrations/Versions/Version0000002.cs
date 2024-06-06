using FluentMigrator;

using System.Data;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Create table to save the recipes' information")]
public class Version0000002 : VersionBase
{
    private const string NAME = "Recipe";

    public override void Up()
    {
        CreateTable(NAME)
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id");

        CreateTable("Ingredients")
            .WithColumn("Item").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredient_Recipe_Id", NAME, "Id")
            .OnDelete(Rule.Cascade);

        CreateTable("Instructions")
            .WithColumn("Step").AsInt32().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Instruction_Recipe_Id", NAME, "Id")
            .OnDelete(Rule.Cascade);

        CreateTable("DishTypes")
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_DishType_Recipe_Id", NAME, "Id")
            .OnDelete(Rule.Cascade);
    }
}
