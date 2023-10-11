using VSATemplate.Features.Common.Data;
using VSATemplate.Features.Common.Entities;
using VSATemplate.Features.Common.Repositories.GenericRepository;
using VSATemplate.Features.Courses.Common.Repository.Base;

namespace VSATemplate.Features.Courses.Common.Repository
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(DataContext dataContext) : base(dataContext) { }
    }
}
