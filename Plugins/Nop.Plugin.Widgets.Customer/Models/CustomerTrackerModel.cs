using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            CustomerPictureModels = new List<CustomerPictureModel>();
            AddPictureModel = new CustomerPictureModel();
            CustomerPictureSearchModel = new CustomerPictureSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.CustomerTracker.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.CustomerTracker.Fields.ContactNo")]
        public string ContactNo { get; set; }

        [NopResourceDisplayName("Admin.CustomerTracker.Fields.Address")]
        public string Address { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Admin.CustomerTracker.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.CustomerTracker.Pictures.Fields.OverrideAltAttribute")]
        public string OverrideAltAttribute { get; set; }

        [NopResourceDisplayName("Admin.Catalog.CustomerTracker.Pictures.Fields.OverrideTitleAttribute")]
        public string OverrideTitleAttribute { get; set; }

        [NopResourceDisplayName("Admin.CustomerTracker.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public string PictureUrl { get; set; }
        [UIHint("Date")]
        public string date { get; set; }

        public CustomerPictureModel AddPictureModel { get; set; }
        public IList<CustomerPictureModel> CustomerPictureModels { get; set; }
        public CustomerPictureSearchModel CustomerPictureSearchModel { get; set; }

        #endregion
    }
     
}