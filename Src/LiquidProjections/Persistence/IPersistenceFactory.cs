namespace LiquidProjections.Persistence
{
    public interface IPersistenceFactory
    {
        IProjectionPersistence<TProjection, TKey> For<TProjection, TKey>() where TProjection : class, IHaveIdentity<TKey>, new();
    }
}
