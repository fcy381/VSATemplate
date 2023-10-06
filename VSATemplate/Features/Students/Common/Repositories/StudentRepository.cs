using VSATemplate.Features.Common.Data;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.GenericRepository;
using VSATemplate.Features.Students.Common.Repository.Base;

namespace VSATemplate.Features.Students.Common.Repository
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(DataContext dataContext) : base(dataContext) { }
    }
}
