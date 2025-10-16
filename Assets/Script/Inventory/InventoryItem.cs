using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int maxStack = 99;
    public string description;
    public ItemType itemType;
}

public enum ItemType
{
    Seed,
    Crop,
    Fish,
    Tool,
    Material
}