using FluentMigrator;

namespace Niles.PrintWeb.Utilities.Migrations
{
    [Migration(202002020004)]
    public class CreatePriceSettings : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("PriceSettings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("TenantId").AsInt32().ForeignKey("Tenant", "Id")
                .WithColumn("Format").AsString().NotNullable()
                .WithColumn("Price").AsDouble().NotNullable();
        }
    }
}