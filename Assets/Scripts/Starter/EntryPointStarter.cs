using System;
using JetBrains.Annotations;
using Manager;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Starter
{
    [UsedImplicitly]
    public sealed class EntryPointStarter : IStartable, IDisposable
    {
        EntryPointManager _manager;

        [Inject]
        public void Init(EntryPointManager manager)
        {
            _manager = manager;
            Debug.Log("EntryPointStarter: initialized on scene load");
        }

        public void Start() => _manager.Start();

        public void Dispose()
        {
            Debug.Log("EntryPointStarter: disposed on scene unload");
        }
    }
}