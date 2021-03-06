using FluentMigrator;

namespace Niles.PrintWeb.Utilities.Migrations
{
    [Migration(202002020001)]
    public class AppendUserRoleColumn : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Column("Role").OnTable("User")
                .AsInt32().NotNullable();
        }
    }
}