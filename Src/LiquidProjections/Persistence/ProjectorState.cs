using System;

namespace LiquidProjections.Persistence
{
    public class ProjectorState : IHaveIdentity<string>
    {
        public string Id { get; set; }
        public long Checkpoint { get; set; }
        public DateTime DateLastUpdatedUtc { get; set; }
    }
}
