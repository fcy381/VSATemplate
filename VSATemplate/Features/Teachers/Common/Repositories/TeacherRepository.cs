using VSATemplate.Features.Common.Data;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.GenericRepository;
using VSATemplate.Features.Courses.Common.Repository.Base;
using VSATemplate.Features.Teachers.Common.Repository.Base;

namespace VSATemplate.Features.Teachers.Common.Repository
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(DataContext dataContext) : base(dataContext) { }
    }
}
