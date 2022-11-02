using Nop.Core.Configuration;

namespace NopStation.Plugin.Widgets.Documentation
{
    public class DocumentationSettings : ISettings
    {
        public string HomepageText { get; set; }

        public bool AllowGuestUsersToLeaveComments { get; set; }

        public bool DocumentationCommentsMustBeApproved { get; set; }

        public int MinimumIntervalToAddComments { get; set; }

        public bool ShowLinkOnTopMenu { get; set; }

        public bool ShowLinkInFooterMenu { get; set; }
    }
}
