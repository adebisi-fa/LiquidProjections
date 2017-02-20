using System;
using System.Threading.Tasks;

namespace LiquidProjections.Persistence
{
    public class EventMapProjectionPersistenceConfigurator<TProjection, TKey>
        where TProjection : class, IHaveIdentity<TKey>, new()
    {
        private readonly IProjectionPersistence<TProjection, TKey> projectionPersistence;
        private readonly IEventMapBuilder<TProjection, TKey, ProjectionContext> eventMapBuilder;

        public EventMapProjectionPersistenceConfigurator(IEventMapBuilder<TProjection, TKey, ProjectionContext> eventMapBuilder, IPersistenceFactory persistenceFactory)
        {
            if (eventMapBuilder == null)
            {
                throw new ArgumentNullException(nameof(eventMapBuilder));
            }

            this.eventMapBuilder = eventMapBuilder;
            eventMapBuilder.HandleProjectionDeletionsAs(HandleProjectionDeletion);
            eventMapBuilder.HandleProjectionModificationsAs(HandleProjectionModification);
            projectionPersistence = persistenceFactory.For<TProjection, TKey>();
        }

        private Task HandleCustomActionAs(ProjectionContext context, Func<Task> projector) => projector();

        public IEventMapBuilder<ProjectionContext> EventMapBuilder => eventMapBuilder;

        private async Task HandleProjectionModification(TKey key, ProjectionContext context,
            Func<TProjection, Task> projector, ProjectionModificationOptions options)
        {
            TProjection projection = await projectionPersistence.GetByIdAsync(key);

            if (projection == null)
            {
                switch (options.MissingProjectionBehavior)
                {
                    case MissingProjectionModificationBehavior.Create:
                        {
                            projection = new TProjection { Id = key };
                            await projector(projection).ConfigureAwait(false);
                            await projectionPersistence.SaveAsync(projection);
                            break;
                        }

                    case MissingProjectionModificationBehavior.Ignore:
                        {
                            break;
                        }

                    case MissingProjectionModificationBehavior.Throw:
                        {
                            throw new ProjectionException(
                                $"Projection {typeof(TProjection)} with key {key} does not exist.");
                        }

                    default:
                        {
                            throw new NotSupportedException(
                                $"Not supported missing projection behavior {options.MissingProjectionBehavior}.");
                        }
                }
            }
            else
            {
                switch (options.ExistingProjectionBehavior)
                {
                    case ExistingProjectionModificationBehavior.Update:
                        {
                            await projector(projection).ConfigureAwait(false);
                            await projectionPersistence.SaveAsync(projection);
                            break;
                        }

                    case ExistingProjectionModificationBehavior.Ignore:
                        {
                            break;
                        }

                    case ExistingProjectionModificationBehavior.Throw:
                        {
                            throw new ProjectionException(
                                $"Projection {typeof(TProjection)} with key {key} already exists.");
                        }

                    default:
                        {
                            throw new NotSupportedException(
                                $"Not supported existing projection behavior {options.ExistingProjectionBehavior}.");
                        }
                }
            }
        }

        private async Task HandleProjectionDeletion(TKey key, ProjectionContext context,
            ProjectionDeletionOptions options)
        {
            TProjection existingProjection = await projectionPersistence.GetByIdAsync(key);

            if (existingProjection == null)
            {
                switch (options.MissingProjectionBehavior)
                {
                    case MissingProjectionDeletionBehavior.Ignore:
                        {
                            break;
                        }

                    case MissingProjectionDeletionBehavior.Throw:
                        {
                            throw new ProjectionException(
                                $"Cannot delete {typeof(TProjection)} projection with key {key}. The projection does not exist.");
                        }

                    default:
                        {
                            throw new NotSupportedException(
                                $"Not supported missing projection behavior {options.MissingProjectionBehavior}.");
                        }
                }
            }
            else
                await projectionPersistence.DeleteAsync(existingProjection);
        }
    }
}
