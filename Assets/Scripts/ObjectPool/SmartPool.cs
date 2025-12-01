using System;
using UnityEngine;

namespace ObjectPool
{
    public class ObjectPool<T> : IDisposable where T : Component, IPoolable
    {
        private readonly Func<T> _factoryMethod;
        private readonly Transform _rootContainer;
        private readonly PoolConfig _config;
        
        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}