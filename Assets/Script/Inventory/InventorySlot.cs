[System.Serializable]
public class InventorySlot
{
    public InventoryItem item;
    public int count;

    public bool IsEmpty => item == null || count <= 0;

    public void Add(InventoryItem newItem, int amount = 1)
    {
        if (item == null)
        {
            item = newItem;
            count = amount;
        }
        else if (item == newItem)
        {
            count += amount;
        }
    }

    public void Remove(int amount = 1)
    {
        count -= amount;
        if (count <= 0)
        {
            item = null;
            count = 0;
        }
    }
}