using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using NopStation.Plugin.Widgets.Documentation.Domains;

namespace NopStation.Plugin.Widgets.Documentation.Admin.Areas.Infrastructure
{
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        public int Order => 1;

        public MapperConfiguration()
        {
            CreateMap<ArticleModel, DocumentArticle>()
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.LimitedToStores, options => options.Ignore());
            CreateMap<DocumentArticle, ArticleModel>()
                .ForMember(model => model.AvailableStores, options => options.Ignore())
                .ForMember(model => model.AvailableCategories, options => options.Ignore())
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.UpdatedOn, options => options.Ignore())
                .ForMember(model => model.SelectedCategoryIds, options => options.Ignore())
                .ForMember(model => model.SelectedStoreIds, options => options.Ignore());

            CreateMap<CategoryModel, DocumentCategory>()
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore())
                .ForMember(entity => entity.LimitedToStores, options => options.Ignore());
            CreateMap<DocumentCategory, CategoryModel>()
                .ForMember(model => model.AvailableStores, options => options.Ignore())
                .ForMember(model => model.AvailableCategories, options => options.Ignore())
                .ForMember(model => model.CreatedOn, options => options.Ignore())
                .ForMember(model => model.UpdatedOn, options => options.Ignore())
                .ForMember(model => model.SelectedStoreIds, options => options.Ignore());

            CreateMap<ArticleComment, ArticleCommentModel>()
                .ForMember(model => model.CreatedOn, options => options.Ignore());
            CreateMap<ArticleCommentModel, ArticleComment>()
                .ForMember(entity => entity.CustomerId, options => options.Ignore())
                .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore());
        }
    }
}
