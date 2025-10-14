
public interface IUnitOfWork : IDisposable
{
    //Dung gi thi add them nhe
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
}
