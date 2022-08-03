using CheckSkills.Web.Services.Interfaces;
using System.Linq.Expressions;

namespace CheckSkills.Web.Services.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T>
        where T : class
    {
        private readonly DataContext _context;

        public RepositoryBase(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                .Where(expression).AsNoTracking();
        }
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
