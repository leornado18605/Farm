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

    private void Awake()
    {
        Instance = this;
        cropPool = new ObjectPoolManager<Crop>(cropPrefab.gameObject, poolParent, 20, 100);
        Prewarm(10); // tạo sẵn 10 cây
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
        crop.gameObject.SetActive(true);

        // 🧠 gọi trực tiếp
        crop.SetSortingOrder(tile.Renderer.sortingOrder + 1);

        activeCrops[tile] = crop;
        return crop;
    }

    public void DespawnCrop(GroundTile tile)
    {
        if (!activeCrops.TryGetValue(tile, out Crop crop)) return;

        cropPool.Release(crop);
        activeCrops.Remove(tile);
    }

    public Crop GetCropAtTile(GroundTile tile)
    {
        activeCrops.TryGetValue(tile, out Crop crop);
        return crop;
    }
}