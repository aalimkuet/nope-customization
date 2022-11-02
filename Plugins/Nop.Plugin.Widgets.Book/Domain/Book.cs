using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.BookTracker.Domain
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }   
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
