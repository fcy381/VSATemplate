using VSATemplate.Features.Common.Entities.Base;

namespace VSATemplate.Features.Common.Entities
{
    public class Course : BaseEntity
    {
        public string? Name { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }
    }
}
