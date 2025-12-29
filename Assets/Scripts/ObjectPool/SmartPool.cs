using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace ObjectPool
{
    public class ObjectPool<T> : IDisposable where T : Component, IPoolable
    {
        private readonly Func<T> _factoryMethod;
        private readonly Transform _rootContainer;
        private readonly PoolConfig _config;
        
        private readonly Stack<T> _inactiveObjects = new();
        private readonly HashSet<T> _activeObjects = new();
        
        private bool _isDisposed;
        private CancellationTokenSource _cts;
        
        public int ActiveCount => _activeObjects.Count;
        public int InactiveCount => _inactiveObjects.Count;

        public ObjectPool(Func<T> factory, PoolConfig config, Transform rootContainer = null)
        {
            _factoryMethod = factory;
            _config = config;
            _rootContainer = rootContainer;
            
            _inactiveObjects = new  Stack<T>(_config.PrewarmCount);

            Prewarm();
            
            // Auto reduction
            _cts = new CancellationTokenSource();
            MaintenanceLoop(_cts.Token).Forget();
        }
        
        private void Prewarm()
        {
            for (var i = 0; i < _config.PrewarmCount; i++)
            {
                var obj = CreateNewInstance();
                obj.gameObject.SetActive(false);
                _inactiveObjects.Push(obj);
            }
        }

        private T CreateNewInstance()
        {
            var instance = _factoryMethod.Invoke();
            instance.transform.SetParent(_rootContainer);
            return instance;
        }

        public T Rent()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(ObjectPool<T>));

            T instance;

            if (_inactiveObjects.Count > 0)
            {
                instance = _inactiveObjects.Pop();
            }
            else
            {
                if (ActiveCount >= _config.MaxCapacity)
                {
                    Debug.LogWarning($"Pool limit reached for {typeof(T).Name}! Returning null or force creating (Choice bro!)");
                }
                
                instance = CreateNewInstance();
            }
            
            _activeObjects.Add(instance);
            instance.gameObject.SetActive(true);
            instance.OnDespawn();
            
            return instance;
        }

        public void Return(T instance)
        {
            
        }

        private async UniTaskVoid MaintenanceLoop(CancellationToken token)
        {
            
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}