
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Plugin.Widgets.Customer.Mapper;
using Nop.Plugin.Widgets.Customer.Models;
using Nop.Plugin.Widgets.CustomerTrackers.Services;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Customer.Factories
{
    /// <summary>
    /// Represents the CustomerTracker model factory implementation
    /// </summary>
    public partial class CustomerTrackerModelFactory : ICustomerTrackerModelFactory
    {
        #region Fields

        private readonly ICustomerTrackerService _CustomerTrackerService;

        #endregion

        #region Ctor

        public CustomerTrackerModelFactory( ICustomerTrackerService CustomerTrackerService )
        {
            _CustomerTrackerService = CustomerTrackerService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare CustomerTracker search model
        /// </summary>
        /// <param name="searchModel">CustomerTracker search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the CustomerTracker search model
        /// </returns>
        public virtual Task<CustomerTrackerSearchModel> PrepareCustomerTrackerSearchModelAsync(CustomerTrackerSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return Task.FromResult(searchModel);
        }

        /// <summary>
        /// Prepare paged CustomerTracker list model
        /// </summary>
        /// <param name="searchModel">CustomerTracker search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the CustomerTracker list model
        /// </returns>
        public virtual async Task<CustomerTrackerListModel> PrepareCustomerTrackerListModelAsync(CustomerTrackerSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get CustomerTrackers
            var CustomerTrackers = await _CustomerTrackerService.GetAllCustomerTrackersAsync(showHidden: true,
                name: searchModel.SearchName,
                roll: searchModel.SearchContactNo,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = await new CustomerTrackerListModel().PrepareToGridAsync(searchModel, CustomerTrackers, () =>
            {
                //fill in model values from the entity
                return CustomerTrackers.SelectAwait(async CustomerTracker =>
                {
                    var CustomerTrackerModel = CustomerTracker.ToModel<CustomerTrackerModel>();
                    return CustomerTrackerModel;
                });
            });

            return model;
        }

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
        public virtual async Task<CustomerTrackerModel> PrepareCustomerTrackerModelAsync(CustomerTrackerModel model, CustomerTracker CustomerTracker, bool excludeProperties = false)
        {

            if (CustomerTracker != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = CustomerTracker.ToModel<CustomerTrackerModel>();
                }
            }

            return model;
        }


        #endregion
    }
}