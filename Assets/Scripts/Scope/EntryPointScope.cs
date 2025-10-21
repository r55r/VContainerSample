using Manager;
using Starter;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public sealed class EntryPointScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EntryPointStarter>();
            builder.Register<EntryPointManager>(Lifetime.Scoped);
        }
    }
}