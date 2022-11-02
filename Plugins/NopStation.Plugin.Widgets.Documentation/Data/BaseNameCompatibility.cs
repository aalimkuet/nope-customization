using System;
using System.Collections.Generic;
using Nop.Data.Mapping;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Data
{
    public class BaseNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string>
        {
            { typeof(DocumentCategory), "NS_Document_Category" },
            { typeof(DocumentArticle), "NS_Document_Article" },
            { typeof(ArticleComment), "NS_Document_ArticleComment" },
            { typeof(ArticleCategory), "NS_Document_Article_Category_Mapping" }
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>
        {
        };
    }
}
