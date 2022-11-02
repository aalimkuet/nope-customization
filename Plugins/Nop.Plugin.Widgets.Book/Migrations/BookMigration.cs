using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.BookTracker.Domain;

namespace Nop.Plugin.Widgets.BookTracker.Data
{
    [NopMigration("2022/08/09 05:40:55", "Widgets.BookTracker base schema", MigrationProcessType.Installation)]
    public class BookMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<Book>();
        }
        
    }

}
