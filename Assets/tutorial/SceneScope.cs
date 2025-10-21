using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace tutorial
{
    public class SceneScope : LifetimeScope
    {
        [SerializeField] private SayHelloButton sayHelloButton;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<HelloWorldService>(Lifetime.Singleton);
            builder.Register<GamePresenter>(Lifetime.Singleton);
            builder.RegisterComponent(sayHelloButton);
            builder.RegisterEntryPoint<GamePresenter>();
        }
    }
}
