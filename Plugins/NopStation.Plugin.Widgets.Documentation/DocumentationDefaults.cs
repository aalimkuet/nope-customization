using Nop.Core.Caching;

namespace NopStation.Plugin.Widgets.Documentation
{
    public class DocumentationDefaults
    {
        public static CacheKey ArticleCategoriesByArticleIdCacheKey => new CacheKey("Nopstation.documentation.category.articlecategory.list.byarticleid-{0}-{1}",
            CategoryCachePrefix, ArticleCachePrefix);
        public static CacheKey CategoriesByParentIdtKey => new CacheKey("Nopstation.documentation.category.list.byparentid-{0}-{1}", CategoryCachePrefix);
        public static CacheKey CategoriesListKey => new CacheKey("Nopstation.documentation.category.list-{0}-{1}-{2}-{3}-{4}", CategoryCachePrefix);
        public static CacheKey CategoriesSelectListKey => new CacheKey("Nopstation.documentation.category.selectlist-{0}", CategoryCachePrefix);
        public static CacheKey CategoryBreadcrumbCacheKey => new CacheKey("Nopstation.documentation.category.breadcrumb-{0}", CategoryCachePrefix);
        public static string CategoryCachePrefix => "Nopstation.documentation.category.";


        public static CacheKey ArticleCommentsListKey => new CacheKey("Nopstation.documentation.article.commentlist-{0}-{1}-{2}-{3}-{4}-{5}-{6}", ArticleCachePrefix);
        public static CacheKey ArticlesListKey => new CacheKey("Nopstation.documentation.article.list-{0}-{1}-{2}-{3}-{4}-{5}", ArticleCachePrefix);
        public static string ArticleCachePrefix => "Nopstation.documentation.article.";

        public static string DocumentArticle => "documentarticle";
        public static string DocumentCategory => "documentcategory";
    }
}
