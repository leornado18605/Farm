using UnityEngine;

public class SeedParticlePool : MonoBehaviour
{
    [SerializeField] private GameObject seedPrefab;
    [SerializeField] private Transform parent;

    private ObjectPoolManager<SeedParticle> pool;

    public static SeedParticlePool Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        pool = new ObjectPoolManager<SeedParticle>(seedPrefab, parent);
    }

    public void SpawnSeed(Vector3 spawnPos, int count = 3)
    {
        for (int i = 0; i < count; i++)
        {
            var seed = pool.Get();
            seed.Init(pool);
            seed.Launch(spawnPos + new Vector3(Random.Range(-0.05f, 0.05f), 0, 0));
        }
    }
}