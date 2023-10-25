namespace VSATemplate.Features.Common.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.Empty;

        public bool IsDeleted { get; set; }

        public DateTime DeletedTimeUtc { get; set; }

        protected BaseEntity()
        {
            IsDeleted = false;
            DeletedTimeUtc = DateTime.MinValue;
        }
    }
}
