using UnityEngine;

namespace ObjectPool
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}

