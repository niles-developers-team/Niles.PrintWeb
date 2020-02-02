using FluentMigrator;

namespace Niles.PrintWeb.Utilities.Migrations
{
    [Migration(202002020002)]
    public class CreateTenant : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Tenants")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("SubscriptionId").AsInt32().NotNullable().ForeignKey("Subscription", "Id")
                .WithColumn("SubscribeDate").AsDateTime().NotNullable();

            Create.Table("Offices")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Address").AsString().NotNullable()
                .WithColumn("TenantId").AsInt32().NotNullable().ForeignKey("Tenant", "Id");
        }
    }
}