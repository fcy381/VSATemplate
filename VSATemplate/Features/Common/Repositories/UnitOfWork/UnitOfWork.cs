using Microsoft.EntityFrameworkCore;
using System.Transactions;
using VSATemplate.Features.Common.Data;
using VSATemplate.Features.Common.Repositories.UnitOfWork.Base;
using VSATemplate.Features.Courses.Common.Repository.Base;
using VSATemplate.Features.Students.Common.Repository.Base;
using VSATemplate.Features.Teachers.Common.Repository.Base;
using static VSATemplate.Features.Students.Commands.CreateStudent;

namespace VSATemplate.Features.Common.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICourseRepository CourseRepository { get; }

        public IStudentRepository StudentRepository { get; }

        public ITeacherRepository TeacherRepository { get; }

        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext, 
                          ICourseRepository courseRepository,
                          IStudentRepository studentRepository,
                          ITeacherRepository teacherRepository)
        {
            CourseRepository = courseRepository; 
            StudentRepository = studentRepository;
            TeacherRepository = teacherRepository;  
            _dataContext = dataContext;
        }

        public async Task<int> Commit()
        => await _dataContext.SaveChangesAsync();

        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}