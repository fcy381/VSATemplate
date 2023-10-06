using VSATemplate.Features.Common.Entities.Base;

namespace VSATemplate.Features.Common.Entities
{
    public class Student : BaseEntity
    {        
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
