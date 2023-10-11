using VSATemplate.Features.Courses.Common.Repository.Base;
using VSATemplate.Features.Students.Common.Repository.Base;
using VSATemplate.Features.Teachers.Common.Repository.Base;
using static VSATemplate.Features.Students.Commands.CreateStudent;

namespace VSATemplate.Features.Common.Repositories.UnitOfWork.Base
{
    public interface IUnitOfWork : IDisposable
    {
        public ICourseRepository CourseRepository { get; }

        public IStudentRepository StudentRepository { get; }

        public ITeacherRepository TeacherRepository { get; }

        Task<int> Commit();
    }
}
