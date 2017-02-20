using LiquidProjections.Persistence;

namespace LiquidProjections.Persistence
{
    public static class PersistenceExtensions
    {
        public static Projector ToProjector<TProjection, TKey> (
            this EventMapProjectionPersistenceConfigurator<TProjection, TKey> configurator,
            ITrackingStore trackingStore, string checkpointId, params Projector[] children
        ) where TProjection : class, IHaveIdentity<TKey>, new()
        {
            return new Projector(configurator.EventMapBuilder, children, trackingStore) { CheckpointId = checkpointId };
        }

        public static EventMapProjectionPersistenceConfigurator<TProjection, TKey> PersistWith<TProjection, TKey>(
            this IEventMapBuilder<TProjection, TKey, ProjectionContext> mapBuilder,
            IPersistenceFactory persistenceFactory
        )
            where TProjection : class, IHaveIdentity<TKey>, new()
        {
            return new EventMapProjectionPersistenceConfigurator<TProjection, TKey>(mapBuilder, persistenceFactory);
        }
    }
}
