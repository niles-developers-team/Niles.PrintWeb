using FluentMigrator;

namespace Niles.PrintWeb.Utilities.Migrations
{
    [Migration(202002020002)]
    public class CreateSubscription : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Subscription")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Price").AsDouble().NotNullable()
                .WithColumn("DaysLimit").AsInt32().NotNullable()
                .WithColumn("ComputersLimit").AsInt32().NotNullable();
        }
    }
}