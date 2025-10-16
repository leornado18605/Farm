using UnityEngine;

public class TestAddItem : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            var item = Resources.Load<InventoryItem>("Fish");
            if (item != null)
            {
                InventoryManager.Instance.AddItem(item, 1);
                Debug.Log($"🧺 Đã thêm {item.itemName} vào túi!");
            }
        }
    }
}