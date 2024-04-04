using Test2.Data;

namespace API_Foreignkey.IReposiory.Repository
{
    public class DBRepository : IDBRepository
    {
        private readonly ApplicationDbContext _context;
        public DBRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
           _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
          return await _context.SaveChangesAsync() > 0;
        }
    }
}
