using System.Threading.Tasks;

namespace LiquidProjections.Testing
{
    public class PassThroughTrackingStore : ITrackingStore
    {
        public Task SaveCheckpoint(string projectorId, long checkpoint) => Task.FromResult(0);
    }
}
