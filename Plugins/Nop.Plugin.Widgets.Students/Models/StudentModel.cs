using System;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.Students.Models
{
    /// <summary>
    /// Represents a Book model
    /// </summary>
    public partial record StudentModel : BaseNopEntityModel 
    {
        #region Ctor

        public StudentModel()
        {
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Students.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Students.Fields.Roll")]
        public string Roll { get; set; }

        [NopResourceDisplayName("Admin.Students.Fields.Session")]
        public DateTime Session { get; set; }  

        [NopResourceDisplayName("Admin.Students.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }        
        #endregion
    }
     
}