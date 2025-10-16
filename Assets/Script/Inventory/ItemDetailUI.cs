using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void ShowDetail(InventoryItem item)
    {
        if (item == null)
        {
            ClearDetail();
            return;
        }

        itemIcon.sprite = item.icon;
        itemIcon.enabled = true;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
    }

    public void ClearDetail()
    {
        itemIcon.enabled = false;
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }
}