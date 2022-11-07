using Nop.Core;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel;

namespace Nop.Plugin.Widgets.BookTracker.Domain
{
    public class CustomerTracker : BaseEntity
    {
        public string Name { get; set; }   
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string OverrideAltAttribute { get; set; }
        public string OverrideTitleAttribute { get; set; }
        public int DisplayOrder { get; set; }
    }
}
