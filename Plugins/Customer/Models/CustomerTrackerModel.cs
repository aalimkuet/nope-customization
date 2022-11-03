using System;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.Customer.Models
{
    /// <summary>
    /// Represents a CustomerTracker model
    /// </summary>
    public partial record CustomerTrackerModel : BaseNopEntityModel 
    {
        #region Ctor

        public CustomerTrackerModel()
        {
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.CustomerTracker.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.CustomerTracker.Fields.ContactNo")]
        public string ContactNo { get; set; }

        [NopResourceDisplayName("Admin.CustomerTracker.Fields.Address")]
        public string Address { get; set; }  

        [NopResourceDisplayName("Admin.CustomerTracker.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }        
        #endregion
    }
     
}