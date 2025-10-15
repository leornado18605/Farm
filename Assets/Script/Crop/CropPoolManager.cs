using UnityEngine;
using System.Collections.Generic;

public class CropPoolManager : MonoBehaviour
{
    public static CropPoolManager Instance;

    [Header("Setup")]
    [SerializeField] private Crop cropPrefab;
    [SerializeField] private Transform poolParent;

    private ObjectPoolManager<Crop> cropPool;
    private readonly Dictionary<GroundTile, Crop> activeCrops = new();
    private readonly Dictionary<Collider2D, Crop> colliderLookup = new(); // 🆕

    private void Awake()
    {
        Instance = this;
        cropPool = new ObjectPoolManager<Crop>(cropPrefab.gameObject, poolParent, 20, 100);
        Prewarm(10);
    }

    private void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Crop c = cropPool.Get();
            cropPool.Release(c);
        }
    }

    public Crop SpawnCrop(GroundTile tile, CropData cropData)
    {
        if (activeCrops.ContainsKey(tile))
            return activeCrops[tile];

        Crop crop = cropPool.Get();
        crop.transform.SetParent(tile.transform);
        crop.transform.localPosition = new Vector3(0, 0.05f, 0);

        crop.Initialize(cropData);
        crop.BindTile(tile);
        crop.gameObject.SetActive(true);
        crop.SetSortingOrder(tile.Renderer.sortingOrder + 1);

        activeCrops[tile] = crop;

        // 🆕 không GetComponent nữa, dùng reference sẵn trong Crop
        if (crop.Collider != null)
            colliderLookup[crop.Collider] = crop;

        return crop;
    }

    public void DespawnCrop(GroundTile tile)
    {
        if (!activeCrops.TryGetValue(tile, out Crop crop)) return;

        if (crop.Collider != null)
            colliderLookup.Remove(crop.Collider);

        cropPool.Release(crop);
        activeCrops.Remove(tile);
    }

    public Crop GetCropAtTile(GroundTile tile)
    {
        activeCrops.TryGetValue(tile, out Crop crop);
        return crop;
    }

    public Crop GetCropByCollider(Collider2D hit)
    {
        colliderLookup.TryGetValue(hit, out Crop crop);
        return crop;
    }
}
