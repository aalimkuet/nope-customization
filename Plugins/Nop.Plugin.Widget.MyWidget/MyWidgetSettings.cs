using Nop.Core.Configuration;

namespace Nop.Plugin.Widget.MyWidget
{
    public class MyWidgetSettings : ISettings
    {
        public int ActiveStoreScopeConfiguration { get; set; }
        public int Id { get; set; }
        public string CustomText { get; set; }
        public bool IsActive { get; set; }
    }
}