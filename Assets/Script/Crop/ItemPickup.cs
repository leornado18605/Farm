using UnityEngine;
using DG.Tweening;

public class ItemPickup : MonoBehaviour
{
    private string itemName;
    private PlayerController player;
    private bool isCollected = false;
    private bool isTargeted = false;

    private const float pickupRange = 0.5f;
    private bool canBeCollected = false;
    
    public void Initialize(string name, PlayerController target)
    {
        itemName = name;
        player = target;
        isCollected = false;
        isTargeted = false;
        
        DOVirtual.DelayedCall(0.5f, () => canBeCollected = true);
    }

    private void Update()
    {
        if (isCollected || player == null || !canBeCollected) return;

        float dist = Vector2.Distance(player.transform.position, transform.position);

        // 👇 Khi click vào item → player đi đến
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(click);
            if (hit != null && hit.gameObject == gameObject)
            {
                isTargeted = true;
                player.MoveToAndAct(transform.position, Collect);
            }
        }

        // 👇 Khi đã target và đến gần
        if (isTargeted && dist <= pickupRange)
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (isCollected) return;
        isCollected = true;

        Debug.Log($"🎒 Player bắt đầu nhặt {itemName}");
        player.StartPickupAnim(this);
    }
    
    // 🪄 Được gọi bởi Player khi anim kết thúc
    public void OnPickedByPlayer()
    {
        Debug.Log($"🍃 {itemName} đã được nhặt hoàn tất!");
        transform.DOKill();
        transform.DOScale(Vector3.zero, 0.25f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                ItemPickupPoolManager.Instance.DespawnItem(this);
            });
    }
}