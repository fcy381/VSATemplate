using static VSATemplate.Features.Students.CreateStudent;

namespace VSATemplate.Repositories.UnitOfWork.Base
{
    public interface IUnitOfWork: IDisposable
    {
        public IStudentRepository StudentRepository { get; } 

        Task<int> Commit();
    }
}
