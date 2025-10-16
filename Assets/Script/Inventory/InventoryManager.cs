using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private int slotCount = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public delegate void OnInventoryChange();
    public event OnInventoryChange OnChanged;

    public InventoryItem selectedItem;
    
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(InventoryItem item, int amount = 1)
    {
        // 1️⃣ Gộp item nếu có slot trùng
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.count < item.maxStack)
            {
                slot.Add(item, amount);
                OnChanged?.Invoke();
                return true;
            }
        }

        // 2️⃣ Tìm slot trống
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.Add(item, amount);
                OnChanged?.Invoke();
                return true;
            }
        }

        Debug.Log("❌ Túi đầy, không thể thêm vật phẩm!");
        return false;
    }

    public void RemoveItem(InventoryItem item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.Remove(amount);
                OnChanged?.Invoke();
                return;
            }
        }
    }
    
    public void SelectItem(InventoryItem item)
    {
        selectedItem = item;
        Debug.Log($" Selected: {item.itemName}");
    }
}