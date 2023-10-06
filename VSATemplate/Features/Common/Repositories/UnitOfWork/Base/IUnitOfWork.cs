using VSATemplate.Features.Students.Common.Repository.Base;
using static VSATemplate.Features.Students.Commands.CreateStudent;

namespace VSATemplate.Features.Common.Repositories.UnitOfWork.Base
{
    public interface IUnitOfWork : IDisposable
    {
        public IStudentRepository StudentRepository { get; }

        Task<int> Commit();
    }
}
