using System.Threading.Tasks;
using Nop.Plugin.Widgets.Students.Domains;
using Nop.Plugin.Widgets.Students.Models;

namespace Nop.Plugin.Widgets.Students.Factories
{
    /// <summary>
    /// Represents the Student model factory
    /// </summary>
    public partial interface IStudentModelFactory
    {
        /// <summary>
        /// Prepare Student search model
        /// </summary>
        /// <param name="searchModel">Student search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Student search model
        /// </returns>
        Task<StudentSearchModel> PrepareStudentSearchModelAsync(StudentSearchModel searchModel);

        /// <summary>
        /// Prepare paged Student list model
        /// </summary>
        /// <param name="searchModel">Student search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Student list model
        /// </returns>
        Task<StudentListModel> PrepareStudentListModelAsync(StudentSearchModel searchModel);

        /// <summary>
        /// Prepare Student model
        /// </summary>
        /// <param name="model">Student model</param>
        /// <param name="Student">Student</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Student model
        /// </returns>
        Task<StudentModel> PrepareStudentModelAsync(StudentModel model, Student student, bool excludeProperties = false);
 
    }
}