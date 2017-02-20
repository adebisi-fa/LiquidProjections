﻿using System;
using System.Threading.Tasks;

namespace LiquidProjections.Persistence
{
    public class TrackingStore : ITrackingStore
    {
        private readonly IPersistenceFactory _factory;
        private readonly IProjectionPersistence<ProjectorState, string> _projectionPersistence;

        public TrackingStore(IPersistenceFactory factory)
        {
            _factory = factory;
            _projectionPersistence = _factory.For<ProjectorState, string>();
        }

        public async Task SaveCheckpoint(string projectorId, long checkpoint)
        {
            Console.WriteLine($"Saving checkpoint with value {checkpoint} for projector {projectorId}");
            await _projectionPersistence.SaveAsync(new ProjectorState
            {
                Checkpoint = checkpoint,
                Id = projectorId,
                DateLastUpdatedUtc = DateTime.UtcNow
            });
        }

        public async Task<ProjectorState> GetCheckpoint(string projectorId)
        {
            Console.WriteLine($"Getting checkpoint for projector {projectorId}");
            return await _projectionPersistence.GetByIdAsync(projectorId);
        }
    }
}
