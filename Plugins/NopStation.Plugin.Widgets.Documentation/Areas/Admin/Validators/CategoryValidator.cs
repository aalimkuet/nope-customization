using FluentValidation;
using NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace NopStation.Plugin.Widgets.Documentation.Admin.Areas.Validators
{
    public class CategoryValidator : BaseNopValidator<CategoryModel>
    {
        #region Ctor

        public CategoryValidator(ILocalizationService localizationService)
        {
            //set validation rules
            RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResourceAsync("Admin.NopStation.Documentation.Categories.Fields.Name.Required").Result);
        }

        #endregion
    }
}