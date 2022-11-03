using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Plugin.Widgets.Customer.Models;
using Nop.Plugin.Widgets.CustomerTrackers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Customer.Services
{
    /// <summary>
    /// CustomerTracker service
    /// </summary>
    public partial class CustomerTrackerService : ICustomerTrackerService
    {
        #region Fields

        private readonly IRepository<CustomerTracker> _CustomerTrackerRepository;

        #endregion

        #region Ctor

        public CustomerTrackerService( 
            IRepository<CustomerTracker> CustomerTrackerRepository )
        {             
            _CustomerTrackerRepository = CustomerTrackerRepository;             
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a CustomerTracker by CustomerTracker identifier
        /// </summary>
        /// <param name="CustomerTrackerId">CustomerTracker identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the CustomerTracker
        /// </returns>
        public virtual async Task<CustomerTracker> GetCustomerTrackerByIdAsync(int CustomerTrackerId)
        {
            return await _CustomerTrackerRepository.GetByIdAsync(CustomerTrackerId, cache => default);
        }

        /// <summary>
        /// Delete a CustomerTracker
        /// </summary>
        /// <param name="CustomerTracker">CustomerTracker</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteCustomerTrackerAsync(CustomerTracker CustomerTracker)
        {
            await _CustomerTrackerRepository.DeleteAsync(CustomerTracker);
        }

        public virtual async Task<IPagedList<CustomerTracker>> GetAllCustomerTrackersAsync(string name = "", string author = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var CustomerTrackers = await _CustomerTrackerRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(v => v.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(author))
                    query = query.Where(v => v.ContactNo.Contains(author));
                query = query.OrderBy(v => v.Name).ThenBy(v => v.ContactNo);

                return query;
            }, pageIndex, pageSize);

            return CustomerTrackers;
        }

        public virtual async Task<List<CustomerTracker>> GetAllCustomerTrackerList(CustomerTracker CustomerTracker)
        {
            //var CustomerTrackers = _CustomerTrackerRepository.GetAllAsync(CustomerTracker);

            var CustomerTrackers = await _CustomerTrackerRepository.GetAllAsync(query =>
            {                  
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);

                return query;
            });

            return (List<CustomerTracker>)CustomerTrackers;
        }

        /// <summary>
        /// Inserts a CustomerTracker
        /// </summary>
        /// <param name="CustomerTracker">CustomerTracker</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertCustomerTrackerAsync(CustomerTracker CustomerTracker)
        {
            await _CustomerTrackerRepository.InsertAsync(CustomerTracker);
        }

        /// <summary>
        /// Updates the CustomerTracker
        /// </summary>
        /// <param name="CustomerTracker">CustomerTracker</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateCustomerTrackerAsync(CustomerTracker CustomerTracker)
        {
            await _CustomerTrackerRepository.UpdateAsync(CustomerTracker);
        }  

        #endregion
    }
}