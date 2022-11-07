using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Plugin.Widgets.Customer.Models;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Customer.Factories
{
    /// <summary>
    /// Represents the CustomerTracker model factory
    /// </summary>
    public partial interface ICustomerTrackerModelFactory
    {
        /// <summary>
        /// Prepare CustomerTracker search model
        /// </summary>
        /// <param name="searchModel">CustomerTracker search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the CustomerTracker search model
        /// </returns>
        Task<CustomerTrackerSearchModel> PrepareCustomerTrackerSearchModelAsync(CustomerTrackerSearchModel searchModel);

        /// <summary>
        /// Prepare paged CustomerTracker list model
        /// </summary>
        /// <param name="searchModel">CustomerTracker search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the CustomerTracker list model
        /// </returns>
        Task<CustomerTrackerListModel> PrepareCustomerTrackerListModelAsync(CustomerTrackerSearchModel searchModel);

        /// <summary>
        /// Prepare CustomerTracker model
        /// </summary>
        /// <param name="model">CustomerTracker model</param>
        /// <param name="CustomerTracker">CustomerTracker</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the CustomerTracker model
        /// </returns>
        Task<CustomerTrackerModel> PrepareCustomerTrackerModelAsync(CustomerTrackerModel model, CustomerTracker CustomerTracker, bool excludeProperties = false);
        Task<CustomerTrackerSearchModel> PrepareCustomerSearchModelAsync(CustomerTrackerSearchModel searchModel);
    }
}