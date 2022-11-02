using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.Students.Domains;

namespace Nop.Plugin.Widgets.Students.Migrations
{
    [NopMigration("2022/10/01 05:40:55", "Widgets.Students base schema", MigrationProcessType.Installation)]
    public class StudentMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<Student>();
        }
        
    }

}
