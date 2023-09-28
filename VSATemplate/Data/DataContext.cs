using Microsoft.EntityFrameworkCore;

namespace VSATemplate.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
