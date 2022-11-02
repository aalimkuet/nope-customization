using Nop.Core;

namespace Nop.Plugin.Widgets.Students.Domains
{
    public partial class Student : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }
        public string Roll { get; set; }
        public string Session { get; set; }     
        public int DisplayOrder { get; set; }
    }
}
 