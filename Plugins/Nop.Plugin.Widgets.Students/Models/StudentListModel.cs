 
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.Students.Models
{
    /// <summary>
    /// Represents a student list model
    /// </summary>
    public partial record StudentListModel : BasePagedListModel<StudentModel>
    {
    }
}