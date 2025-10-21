using System;
using Service;
using State;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scope
{
    public sealed class RootProjectScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<GameStateSerializer>(Lifetime.Singleton)
                .AsSelf() // To load game state only
                .AsImplementedInterfaces(); // To all other consumers
            builder.Register<DebtService>(Lifetime.Singleton);
            builder.Register<CoinService>(Lifetime.Singleton);
            builder.Register(resolver =>
            {
                var serializer = resolver.Resolve<GameStateSerializer>();
                return serializer.LoadGameState();
            }, Lifetime.Singleton);
            // Shortcuts required to pass specific state parts in services, not a complete game state
            RegisterGameStateShortcut(builder, gs => gs.clickState);
            RegisterGameStateShortcut(builder, gs => gs.upgradeState);
            RegisterGameStateShortcut(builder, gs => gs.coinState);
            if (Application.isEditor)
            {
                builder.Register<IAnalyticsService, EditorAnalytics>(Lifetime.Singleton);
            }
            else
            {
                builder.Register<IAnalyticsService, RuntimeAnalytics>(Lifetime.Singleton);
            }
        }

        void RegisterGameStateShortcut<T>(IContainerBuilder builder, Func<GameState, T> container) where T : class
        {
            builder.Register(resolver =>
            {
                var gameState = resolver.Resolve<GameState>();
                return container(gameState);
            }, Lifetime.Singleton);
        }
    }
}