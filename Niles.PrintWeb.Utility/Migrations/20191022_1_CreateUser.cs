using FluentMigrator;

namespace Niles.PrintWeb.Utility.Migrations
{
    [Migration(201910220001)]
    public class CreateUser : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserName").AsString().NotNullable()
                .WithColumn("PasswordHash").AsString().NotNullable()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("DateUpdated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

                
            Create.Table("NotConfirmedUser")
                .WithColumn("UserId").AsInt32().PrimaryKey().ForeignKey("User", "Id")
                .WithColumn("ConfirmCode").AsGuid().NotNullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }
        
        public override void Down()
        {
            Delete.Table("Users");
            Delete.Table("NotConfirmedUser");
        }
    }
}