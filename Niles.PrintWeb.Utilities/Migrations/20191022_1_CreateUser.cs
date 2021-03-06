using FluentMigrator;

namespace Niles.PrintWeb.Utilities.Migrations
{
    [Migration(201910220001)]
    public class CreateUser : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserName").AsString().NotNullable()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("ConfirmCode").AsGuid().Nullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("DateUpdated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }
    }
}