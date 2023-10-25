using VSATemplate.Features.Common.Entities.Base;

namespace VSATemplate.Features.Common.Repositories.GenericRepository.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Create(T entity);

        Task<T?> GetById(params object[] keys);

        Task<bool?> WasSoftDeleted(Guid id);

        IQueryable<T> GetAll();

        void Update(T entity);

        Task<bool> HardDelete(Guid id);

        Task<bool> SoftDelete(Guid id);     
    }
}
