using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Widgets.Students.Domains;
using Nop.Plugin.Widgets.Students.Models;

namespace Nop.Plugin.Widgets.Students.Mapping
{
    /// <summary>
    /// AutoMapper configuration for admin area models
    /// </summary>
    public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public AdminMapperConfiguration()
        {
            //create specific maps             
            CreateStudentsMaps(); 
        }

        #endregion

        #region Utilities


        /// <summary>
        /// Create Students maps 
        /// </summary>
        protected virtual void CreateStudentsMaps()
        {
            CreateMap<StudentModel, Student>();               
            CreateMap<Student, StudentModel>();                 
           
        }

        #endregion

        #region Properties

        /// <summary>
        /// Order of this mapper implementation
        /// </summary>
        public int Order => 0;

        #endregion
    }
}