using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.Students
{
    public class StudentWidgetSettings : ISettings
    {
        public int ActiveStoreScopeConfiguration { get; set; }
        public int Id { get; set; }
        public string CustomText { get; set; }
        public bool IsActive { get; set; }
    }
}