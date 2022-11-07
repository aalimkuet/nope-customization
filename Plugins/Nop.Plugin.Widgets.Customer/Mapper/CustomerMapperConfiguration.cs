using AutoMapper;
using Nop.Core.Domain.Catalog;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Widgets.BookTracker.Domain;
using Nop.Plugin.Widgets.Customer.Domain;
using Nop.Plugin.Widgets.Customer.Models;

namespace Nop.Plugin.Widgets.Customer.Mapper
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public class CustomerMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public CustomerMapperConfiguration()
        {
            //create specific maps             
            CreateCustomerTrackersMaps(); 
        }

        #endregion

        #region Utilities


        /// <summary>
        /// Create CustomerTrackers maps 
        /// </summary>
        protected virtual void CreateCustomerTrackersMaps()
        {
            CreateMap<CustomerTrackerModel, CustomerTracker>();               
            CreateMap<CustomerTracker, CustomerTrackerModel>();

            //CreateMap<CustomerPicture, CustomerPictureModel>();
            //CreateMap<CustomerPictureModel, CustomerPicture>();
              
        }

        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;

        #endregion
    }
}