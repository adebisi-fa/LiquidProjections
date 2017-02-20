using FluidCaching;
using Raven.Client;
using Raven.Client.Embedded;

namespace LiquidProjections.RavenDB.Specs
{
    internal class InMemoryRavenDbBuilder
    {
        public IDocumentStore Build()
        {
            var store = new EmbeddableDocumentStore { RunInMemory = true };
            store.Configuration.Storage.Voron.AllowOn32Bits = true;
            return store.Initialize();
        }
    }
}