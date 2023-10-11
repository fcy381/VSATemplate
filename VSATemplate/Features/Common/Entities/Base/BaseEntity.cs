namespace VSATemplate.Features.Common.Entities.Base
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeletedTimeUtc { get; set; }

        public BaseEntity()
        {
          IsDeleted = false;
        }
    }
}
