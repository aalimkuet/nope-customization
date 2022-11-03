using Nop.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.BookTracker.Domain
{
    public class CustomerTracker : BaseEntity
    {
        public string Name { get; set; }   
        [DisplayName("Contact No")]
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public int DisplayOrder { get; set; }

    }
}
