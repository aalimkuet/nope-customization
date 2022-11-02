using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Data
{
    [NopMigration("2021/04/01 08:37:55:1687541", "NopStation.Documentation base scheme", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<DocumentCategory>();
            Create.TableFor<DocumentArticle>();
            Create.TableFor<ArticleComment>();
            Create.TableFor<ArticleCategory>();
        }
    }
}
