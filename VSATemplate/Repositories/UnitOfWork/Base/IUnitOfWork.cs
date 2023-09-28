namespace VSATemplate.Repositories.UnitOfWork.Base
{
    public interface IUnitOfWork: IDisposable
    {
        Task<int> Commit();
    }
}
