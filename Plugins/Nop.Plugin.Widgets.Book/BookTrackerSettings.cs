using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.BookTracker
{
    public class BookTrackerSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to limit shipping methods to configured ones
        /// </summary>
        public bool LimitMethodsToCreated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "shipping calculation by weight and by total" method is selected
        /// </summary>
        public bool ShippingByWeightByTotalEnabled { get; set; }
    }
}
