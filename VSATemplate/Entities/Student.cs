using VSATemplate.Entities.Base;

namespace VSATemplate.Entities
{
    public class Student : BaseEntity
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
