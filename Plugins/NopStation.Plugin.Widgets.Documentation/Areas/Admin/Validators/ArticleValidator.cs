using FluentValidation;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace NopStation.Plugin.Widgets.Documentation.Admin.Areas.Validators
{
    public class ArticleValidator : BaseNopValidator<ArticleModel>
    {
        #region Ctor

        public ArticleValidator(ILocalizationService localizationService)
        {
            //set validation rules
            RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.Fields.Name.Required").Result);
            RuleFor(model => model.Description)
                 .NotEmpty()
                 .WithMessage(localizationService.GetResourceAsync("Admin.NopStation.Documentation.Articles.Fields.Description.Required").Result);
        }

        #endregion
    }
}