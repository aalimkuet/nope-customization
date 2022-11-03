using Nop.Core;
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Plugin.Widgets.Customer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.CustomerTrackers.Services
{
    /// <summary>
    /// CustomerTracker service interface
    /// </summary>
    public partial interface ICustomerTrackerService
    {
        /// <summary>
        /// Gets a CustomerTracker by CustomerTracker identifier
        /// </summary>
        /// <param name="CustomerTrackerId">CustomerTracker identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the CustomerTracker
        /// </returns>
        Task<CustomerTracker> GetCustomerTrackerByIdAsync(int CustomerTrackerId);

        /// <summary>
        /// Delete a CustomerTracker
        /// </summary>
        /// <param name="CustomerTracker">CustomerTracker</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteCustomerTrackerAsync(CustomerTracker CustomerTracker);

        /// <summary>
        /// Gets all CustomerTrackers
        /// </summary>
        /// <param name="name">CustomerTracker name</param>
        /// <param name="roll">CustomerTracker email</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the CustomerTrackers
        /// </returns>
        Task<IPagedList<CustomerTracker>> GetAllCustomerTrackersAsync(string name = "", string roll = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a CustomerTracker
        /// </summary>
        /// <param name="CustomerTracker">CustomerTracker</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertCustomerTrackerAsync(CustomerTracker CustomerTracker);

        /// <summary>
        /// Updates the CustomerTracker
        /// </summary>
        /// <param name="CustomerTracker">CustomerTracker</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateCustomerTrackerAsync(CustomerTracker CustomerTracker);
         Task<List<CustomerTracker>> GetAllCustomerTrackerList(CustomerTracker CustomerTracker);
    }
}