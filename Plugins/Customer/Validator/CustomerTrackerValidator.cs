using Nop.Core.Domain.Orders;
using Nop.Data.Mapping;
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.Customer.Validator
{
    public partial class CustomerTrackerValidator : BaseNopValidator<CustomerTracker>
    {
        public CustomerTrackerValidator(ILocalizationService localizationService, IMappingEntityAccessor mappingEntityAccessor)
        {
            //RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.ReturnRequests.Fields.ReasonForReturn.Required"));
            //RuleFor(x => x.ContactNo).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.ReturnRequests.Fields.RequestedAction.Required"));
            //RuleFor(x => x.Address).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.ReturnRequests.Fields.Quantity.Required"));
             
            SetDatabaseValidationRules<ReturnRequest>(mappingEntityAccessor);
        }
    }
}
