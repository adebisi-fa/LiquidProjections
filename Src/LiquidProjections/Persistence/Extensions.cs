using LiquidProjections.Persistence;

namespace LiquidProjections.Persistence
{
    public static class PersistenceExtensions
    {
        public static Projector ProjectTo<TProjection, TKey>(this IEventMapBuilder<TProjection, TKey, ProjectionContext> mapBuilder,
            IPersistenceFactory persistenceFactory,
            string trackingCheckpointId = null
        )
            where TProjection : class, IHaveIdentity<TKey>, new()
        {
            return new Projector(
                new EventMapProjectionPersistenceConfigurator<TProjection, TKey>(mapBuilder, persistenceFactory).EventMapBuilder,
                null,
                new TrackingStore(persistenceFactory)
            )
            { CheckpointId = trackingCheckpointId ?? TrackingStore.DefaultCheckpointTrackingId };
        }
    }
}
