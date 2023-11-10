using IdentityModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using VSATemplate.Features.Common.Entities.Base;

namespace VSATemplate.Features.Common.Repositories.GenericRepository.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(CancellationToken cancellationToken = default, params object[] keys);

        Task<bool> IsExistsAsync(Expression<Func<T, bool>> querySelector, CancellationToken cancellationToken = default);

        Task<T?> GetAsync(Expression<Func<T, bool>> querySelector, CancellationToken cancellationToken = default);

        void UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool?> WasSoftDeletedAsync(Guid id, CancellationToken cancellationToken = default);

        IQueryable<T> GetAllAsync(CancellationToken cancellationToken = default);

        IQueryable<T> ExecuteQuery([Optional] Expression<Func<T, bool>>? querySelector);
    }
}
