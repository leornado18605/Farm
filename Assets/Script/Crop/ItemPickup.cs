using UnityEngine;
using DG.Tweening;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Collider2D myCollider;
    private string itemName;
    private PlayerController player;
    private bool isCollected = false;
    private bool isTargeted = false;

    private const float pickupRange = 0.5f;
    private bool canBeCollected = false;
    
    public Collider2D Collider => myCollider;
    
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

        if (isTargeted && dist <= pickupRange)
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (isCollected) return;
        isCollected = true;

        player.StartPickupAnim(this);
    }
    
    public void OnPickedByPlayer()
    {
        transform.DOKill();
        transform.DOScale(Vector3.zero, 0.25f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                ItemPickupPoolManager.Instance.DespawnItem(this);
            });
        
        InventoryItem itemData = Resources.Load<InventoryItem>($"Items/{itemName}");
        if (itemData != null)
            InventoryManager.Instance.AddItem(itemData, 1);
    }
    
    public enum CollectableType
    {
        None,
        Wood
    }
}