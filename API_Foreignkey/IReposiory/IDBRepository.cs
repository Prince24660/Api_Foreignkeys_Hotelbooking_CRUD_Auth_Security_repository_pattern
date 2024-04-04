namespace API_Foreignkey.IReposiory
{
    public interface IDBRepository
    {
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }
}
