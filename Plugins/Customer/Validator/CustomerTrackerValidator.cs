using FluentValidation;
using Nop.Data.Mapping;
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Plugin.Widgets.Customer.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.Customer.Validator
{
    public partial class CustomerTrackerValidator : BaseNopValidator<CustomerTrackerModel>
    {
        public CustomerTrackerValidator(ILocalizationService localizationService, IMappingEntityAccessor mappingEntityAccessor)
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessageAwait(localizationService
                .GetResourceAsync("Admin.CustomerTrackers.Fields.Name.Required"));
            RuleFor(x => x.ContactNo)
                .NotEmpty()
                .WithMessageAwait(localizationService
                .GetResourceAsync("Admin.CustomerTrackers.Fields.ContactNo.Required"));
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessageAwait(localizationService
                .GetResourceAsync("Admin.CustomerTrackers.Fields.Address.Required"));

            SetDatabaseValidationRules<CustomerTracker>(mappingEntityAccessor);
        }
    }
}
