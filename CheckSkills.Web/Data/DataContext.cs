using CheckSkills.Web.Models;

namespace CheckSkills.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<MessageWrapper> MessageWrappers { get; set; }
    }
}
