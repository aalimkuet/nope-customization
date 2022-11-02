using Nop.Plugin.Widgets.Students.Domains;
using Nop.Plugin.Widgets.Students.Mapping;
using Nop.Plugin.Widgets.Students.Models;
using Nop.Plugin.Widgets.Students.Services;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.Students.Factories
{
    /// <summary>
    /// Represents the Student model factory implementation
    /// </summary>
    public partial class StudentModelFactory : IStudentModelFactory
    {
        #region Fields

        private readonly IStudentService _StudentService;

        #endregion

        #region Ctor

        public StudentModelFactory( IStudentService StudentService )
        {
            _StudentService = StudentService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare Student search model
        /// </summary>
        /// <param name="searchModel">Student search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Student search model
        /// </returns>
        public virtual Task<StudentSearchModel> PrepareStudentSearchModelAsync(StudentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return Task.FromResult(searchModel);
        }

        /// <summary>
        /// Prepare paged Student list model
        /// </summary>
        /// <param name="searchModel">Student search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Student list model
        /// </returns>
        public virtual async Task<StudentListModel> PrepareStudentListModelAsync(StudentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get Students
            var Students = await _StudentService.GetAllStudentsAsync(showHidden: true,
                name: searchModel.SearchName,
                roll: searchModel.SearchRoll,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = await new StudentListModel().PrepareToGridAsync(searchModel, Students, () =>
            {
                //fill in model values from the entity
                return Students.SelectAwait(async student =>
                {
                    var StudentModel = student.ToModel<StudentModel>();
                    return StudentModel;
                });
            });

            return model;
        }

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
        public virtual async Task<StudentModel> PrepareStudentModelAsync(StudentModel model, Student student, bool excludeProperties = false)
        {

            if (student != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    model = student.ToModel<StudentModel>();
                }
            }

            return model;
        }


        #endregion
    }
}