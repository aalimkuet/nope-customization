using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Books;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.Students.Domains;
using System;

namespace Nop.Plugin.Widgets.Students.Mapping
{
    internal class StudentBuilder : NopEntityBuilder<Student>
    {   
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Student.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(Student.Roll)).AsString(400).Nullable()
                .WithColumn(nameof(Student.Session)).AsString(400).Nullable();
        }
    }
}
