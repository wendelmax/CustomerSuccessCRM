namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IBaseRepository
    {
        Task<bool> SaveChangesAsync();
        Task<bool> ExisteAsync(int id);
        Task<bool> AdicionarAsync(object entidade);
        Task<bool> AtualizarAsync(object entidade);
        Task<bool> DeletarAsync(int id);
    }
} 