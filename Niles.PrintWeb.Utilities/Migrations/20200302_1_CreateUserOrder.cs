using FluentMigrator;

namespace Niles.PrintWeb.Utilities.Migrations
{
    [Migration(202003020001)]
    public class CreateUserOrder : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("UserOrder")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("User", "Id")
                .WithColumn("TenantId").AsInt32().NotNullable().ForeignKey("Tenant", "Id")
                .WithColumn("Total").AsDouble().NotNullable()
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("DateUpdated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
            
            Create.Table("OrderDetail")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("UserOrderId").AsInt32().NotNullable().ForeignKey("UserOrder", "Id")
                .WithColumn("StartPage").AsInt32().NotNullable()
                .WithColumn("EndPage").AsInt32().NotNullable()
                .WithColumn("OnlyBlackAndWhite").AsBoolean().NotNullable()
                .WithColumn("FileName").AsString().NotNullable()
                .WithColumn("FileData").AsBinary().Nullable()
                .WithColumn("FileLength").AsInt32().Nullable()
                .WithColumn("PrintStatus").AsInt32().Nullable()
                .WithColumn("PrintFormat").AsInt32().Nullable();
        }
    }
}