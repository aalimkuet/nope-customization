using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.Widgets.Students.Domains;

namespace Nop.Plugin.Widgets.Students.Services
{
    /// <summary>
    /// Student service interface
    /// </summary>
    public partial interface IStudentService
    {
        /// <summary>
        /// Gets a Student by Student identifier
        /// </summary>
        /// <param name="StudentId">Student identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Student
        /// </returns>
        Task<Student> GetStudentByIdAsync(int studentId);

        /// <summary>
        /// Delete a Student
        /// </summary>
        /// <param name="Student">Student</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteStudentAsync(Student Student);

        /// <summary>
        /// Gets all Students
        /// </summary>
        /// <param name="name">Student name</param>
        /// <param name="roll">Student email</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Students
        /// </returns>
        Task<IPagedList<Student>> GetAllStudentsAsync(string name = "", string roll = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a Student
        /// </summary>
        /// <param name="Student">Student</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertStudentAsync(Student student);

        /// <summary>
        /// Updates the Student
        /// </summary>
        /// <param name="Student">Student</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateStudentAsync(Student Student);
         Task<List<Student>> GetAllStudentList(Student student);
    }
}