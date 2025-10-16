using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button slotButton;           // ← Gắn sẵn trong Inspector
    [SerializeField] private ItemDetailUI itemDetailUI;   // ← Gắn sẵn panel hiển thị chi tiết

    private InventorySlot currentSlot;

    private void Awake()
    {
        if (slotButton != null)
            slotButton.onClick.AddListener(OnSlotClicked);
    }

    public void SetSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (slot == null || slot.IsEmpty)
        {
            icon.enabled = false;
            countText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = slot.item.icon;
        countText.text = slot.count.ToString();
    }

    private void OnSlotClicked()
    {
        if (currentSlot == null || currentSlot.item == null) return;
        if (itemDetailUI != null)
            itemDetailUI.ShowDetail(currentSlot.item);
    }
    
    public void InjectDetailUI(ItemDetailUI detailUI)
    {
        itemDetailUI = detailUI;
    }

}