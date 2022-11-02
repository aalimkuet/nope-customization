using Microsoft.AspNetCore.Mvc;
using NopStation.Plugin.Widgets.Documentation.Models;
using Nop.Web.Framework.Components;

namespace NopStation.Plugin.Widgets.Documentation.Components
{
    public class DocumentationViewComponent : NopViewComponent
    {
        private readonly DocumentationSettings _documentationSettings;

        public DocumentationViewComponent(DocumentationSettings documentationSettings)
        {
            _documentationSettings = documentationSettings;
        }

        public IViewComponentResult Invoke()
        {
            var model = new MenuModel
            {
                ShowLinkInFooterMenu = _documentationSettings.ShowLinkInFooterMenu,
                ShowLinkOnTopMenu = _documentationSettings.ShowLinkOnTopMenu,
            };
            return View(model);
        }
    }
}