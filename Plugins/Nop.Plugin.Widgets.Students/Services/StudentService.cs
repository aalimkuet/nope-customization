using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Widgets.Students.Domains;

namespace Nop.Plugin.Widgets.Students.Services
{
    /// <summary>
    /// Student service
    /// </summary>
    public partial class StudentService : IStudentService
    {
        #region Fields

        private readonly IRepository<Student> _StudentRepository;

        #endregion

        #region Ctor

        public StudentService( 
            IRepository<Student> StudentRepository )
        {             
            _StudentRepository = StudentRepository;             
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a Student by Student identifier
        /// </summary>
        /// <param name="StudentId">Student identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Student
        /// </returns>
        public virtual async Task<Student> GetStudentByIdAsync(int StudentId)
        {
            return await _StudentRepository.GetByIdAsync(StudentId, cache => default);
        }

        /// <summary>
        /// Delete a Student
        /// </summary>
        /// <param name="Student">Student</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteStudentAsync(Student Student)
        {
            await _StudentRepository.DeleteAsync(Student);
        }

        public virtual async Task<IPagedList<Student>> GetAllStudentsAsync(string name = "", string author = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var Students = await _StudentRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(v => v.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(author))
                    query = query.Where(v => v.Roll.Contains(author));
                query = query.OrderBy(v => v.Name).ThenBy(v => v.Roll);

                return query;
            }, pageIndex, pageSize);

            return Students;
        }

        public virtual async Task<List<Student>> GetAllStudentList(Student Student)
        {
            //var Students = _StudentRepository.GetAllAsync(Student);

            var Students = await _StudentRepository.GetAllAsync(query =>
            {                  
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);

                return query;
            });

            return (List<Student>)Students;
        }

        /// <summary>
        /// Inserts a Student
        /// </summary>
        /// <param name="Student">Student</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertStudentAsync(Student Student)
        {
            await _StudentRepository.InsertAsync(Student);
        }

        /// <summary>
        /// Updates the Student
        /// </summary>
        /// <param name="Student">Student</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateStudentAsync(Student Student)
        {
            await _StudentRepository.UpdateAsync(Student);
        }

    

        #endregion
    }
}