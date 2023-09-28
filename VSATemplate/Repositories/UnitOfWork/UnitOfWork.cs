using Microsoft.EntityFrameworkCore;
using System.Transactions;
using VSATemplate.Data;
using VSATemplate.Repositories.UnitOfWork.Base;

namespace VSATemplate.Repositories.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
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