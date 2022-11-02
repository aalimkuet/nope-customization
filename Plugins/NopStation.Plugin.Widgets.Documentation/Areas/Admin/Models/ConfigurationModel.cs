using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopStation.Plugin.Widgets.Documentation.Areas.Admin.Models
{
    public record ConfigurationModel : BaseNopModel, ISettingsModel
    {
        [NopResourceDisplayName("Admin.NopStation.Documentation.Configuration.Fields.HomepageText")]
        public string HomepageText { get; set; }
        public bool HomepageText_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Configuration.Fields.AllowGuestUsersToLeaveComments")]
        public bool AllowGuestUsersToLeaveComments { get; set; }
        public bool AllowGuestUsersToLeaveComments_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Configuration.Fields.DocumentationCommentsMustBeApproved")]
        public bool DocumentationCommentsMustBeApproved { get; set; }
        public bool DocumentationCommentsMustBeApproved_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Configuration.Fields.MinimumIntervalToAddComments")]
        public int MinimumIntervalToAddComments { get; set; }
        public bool MinimumIntervalToAddComments_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Configuration.Fields.ShowLinkOnTopMenu")]
        public bool ShowLinkOnTopMenu { get; set; }
        public bool ShowLinkOnTopMenu_OverrideForStore { get; set; }

        [NopResourceDisplayName("Admin.NopStation.Documentation.Configuration.Fields.ShowLinkInFooterMenu")]
        public bool ShowLinkInFooterMenu { get; set; }
        public bool ShowLinkInFooterMenu_OverrideForStore { get; set; }

        public int ActiveStoreScopeConfiguration { get; set; }
    }
}
