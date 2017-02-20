using System.Threading.Tasks;

namespace LiquidProjections.Persistence
{
    public interface IProjectionPersistence<TProjection, in TKey>
        where TProjection : class, IHaveIdentity<TKey>, new()
    {
        Task<TProjection> GetByIdAsync(TKey id);
        Task SaveAsync(TProjection projection);
        Task DeleteAsync(TProjection projection);
        Task DeleteByIdAsync(TKey projectionId);
    }
}
