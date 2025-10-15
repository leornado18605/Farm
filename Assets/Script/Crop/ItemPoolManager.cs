using UnityEngine;
using System.Collections.Generic;

public class ItemPickupPoolManager : MonoBehaviour
{
    public static ItemPickupPoolManager Instance;

    [Header("Setup")]
    [SerializeField] private ItemPickup itemPrefab;
    [SerializeField] private Transform poolParent;
    [SerializeField] private PlayerController playerRef; // ✅ Inject sẵn player

    private ObjectPoolManager<ItemPickup> itemPool;
    private readonly List<ItemPickup> activeItems = new();

    private void Awake()
    {
        Instance = this;
        itemPool = new ObjectPoolManager<ItemPickup>(itemPrefab.gameObject, poolParent, 20, 100);
        Prewarm(10);
    }

    private void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ItemPickup iPickup = itemPool.Get();
            itemPool.Release(iPickup);
        }
    }

    public ItemPickup SpawnItem(Vector3 pos, string itemName)
    {
        ItemPickup item = itemPool.Get();
        item.transform.SetParent(poolParent);
        item.transform.position = pos;
        item.Initialize(itemName, playerRef);
        activeItems.Add(item);
        return item;
    }

    public void DespawnItem(ItemPickup item)
    {
        if (activeItems.Contains(item))
            activeItems.Remove(item);

        itemPool.Release(item);
    }
}