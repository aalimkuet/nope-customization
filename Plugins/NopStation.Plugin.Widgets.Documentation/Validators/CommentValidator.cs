using FluentValidation;
using NopStation.Plugin.Widgets.Documentation.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace NopStation.Plugin.Widgets.Documentation.Validators
{
    public class CommentValidator : BaseNopValidator<ArticleCommentModel>
    {
        #region Ctor

        public CommentValidator(ILocalizationService localizationService)
        {
            //set validation rules
            RuleFor(model => model.CommentText)
                .NotEmpty()
                .WithMessage(localizationService.GetResourceAsync("NopStation.Documentation.Article.Comments.Fields.CommentText.Required").Result);
        }

        #endregion
    }
}