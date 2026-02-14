namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> SaveChangesAsync();
        Task<bool> ExisteAsync(int id);
        Task<bool> AdicionarAsync(T entidade);
        Task<bool> AtualizarAsync(T entidade);
        Task<bool> DeletarAsync(int id);
    }
} 