using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private ItemDetailUI itemDetailUI;

    private bool isOpen = false;

    private void Start()
    {
        InventoryManager.Instance.OnChanged += Refresh;
        inventoryPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
            Refresh();
    }

    private void Refresh()
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        foreach (var slot in InventoryManager.Instance.slots)
        {
            var go = Instantiate(slotPrefab, slotParent);
            var ui = go.GetComponent<InventorySlotUI>();
            ui.SetSlot(slot);
            
            ui.InjectDetailUI(itemDetailUI);
        }
    }
}