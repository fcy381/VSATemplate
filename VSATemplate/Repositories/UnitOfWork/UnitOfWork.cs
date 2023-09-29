using Microsoft.EntityFrameworkCore;
using System.Transactions;
using VSATemplate.Data;
using VSATemplate.Repositories.UnitOfWork.Base;
using static VSATemplate.Features.Students.CreateStudent;

namespace VSATemplate.Repositories.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        public IStudentRepository StudentRepository { get; }

        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext, IStudentRepository studentRepository)
        {
            StudentRepository = studentRepository;
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