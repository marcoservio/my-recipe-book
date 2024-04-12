using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USER, "Create table to save the user's information")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString(45).NotNullable()
            .WithColumn("Email").AsString(45).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }
}
