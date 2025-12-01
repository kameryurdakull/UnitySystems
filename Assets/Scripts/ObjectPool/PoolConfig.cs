using System;

namespace ObjectPool
{
    [Serializable]
    public struct PoolConfig
    {
        public int PrewarmCount; // Başlangıçta kaç obje olsun
        public int MaxCapacity; // Hard limit (opsiyonel, sonsuz büyüme riskli olabilir)
        public int ShrinkThreshold; // Havuz boyutu bunu geçerse küçülme başlar
        public float ShrinkCheckRate; // Kaç saniyede bir kontrol etsin
        public float ShrinkRate; // Her kontrolde yüzde kaçını yok etsin (0.1f = %10)

        public static PoolConfig Default => new PoolConfig
        {
            PrewarmCount = 10,
            MaxCapacity = 500,
            ShrinkThreshold = 20,
            ShrinkCheckRate = 5.0f,
            ShrinkRate = 0.2f
        };
    }
}