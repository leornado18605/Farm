using UnityEngine;
using DG.Tweening;
public class Crop : MonoBehaviour
{
    [Header("Crop Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D myCollider;   // 🆕 Collider reference
    public Collider2D Collider => myCollider;
    [SerializeField] private CropData cropData;
    [SerializeField] private Animator animator;
    public SpriteRenderer Renderer => spriteRenderer;
    public GameObject treePickupPrefab;
    
    private int currentStage = 0;
    private bool isGrowing = false;
    private float timer = 0f;
    private GroundTile currentTile;

    private bool isCutDown = false; 
    public bool IsCutDown => isCutDown;

    public void BindTile(GroundTile tile)
    {
        currentTile = tile;
    }

    public bool IsMature => currentStage >= cropData.growthSprites.Length - 1;


    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(CropData data)
    {
        cropData = data;
        currentStage = 0;
        timer = 0;
        isGrowing = true;

        spriteRenderer.sprite = cropData.growthSprites[0];
    }

    public void SetSortingOrder(int order)
    {
        spriteRenderer.sortingOrder = order;
    }

    private void Update()
    {
        if (!isGrowing || IsMature) return;

        timer += Time.deltaTime;
        if (timer >= cropData.timePerStage)
        {
            timer = 0f;
            AdvanceGrowthStage();
        }
    }

    private void AdvanceGrowthStage()
    {
        if (currentStage < cropData.growthSprites.Length - 1)
        {
            currentStage++;
            spriteRenderer.sprite = cropData.growthSprites[currentStage];
        }

        if (IsMature)
        {
            isGrowing = false;
        }
    }
    
    public void TryHarvest(PlayerController player)
    {
        if (!IsMature)
        {
            Debug.Log("⚠️ Cây chưa chín để thu hoạch!");
            return;
        }

        Debug.Log($"✅ {cropData.cropName} được thu hoạch!");
        SpawnHarvestEffect();
        StartHarvestAnim(player);
    }
    
    public void TryRemoveAfterCut()
    {
        if (!isCutDown) return;

        CropPoolManager.Instance.DespawnCrop(currentTile);
        isCutDown = false;
    }

    private void SpawnHarvestEffect()
    {
    }
    public GroundTile GetBoundTile()
    {
        return currentTile;
    }
    public void StartHarvestAnim(PlayerController player)
    {
        if (animator != null)
        {
            animator.SetTrigger("Harvested"); 
        }
        else
        {
            OnHarvestAnimationEnd();
        }
    }

    public void OnHarvestAnimationEnd()
    {
        if (currentTile != null)
            currentTile.SetState(SoilState.Hoed);

        Vector3 dropPos = transform.position + new Vector3(0, -0.75f, 0);
        ItemPickupPoolManager.Instance.SpawnItem(dropPos, "Tree");

        CropPoolManager.Instance.DespawnCrop(currentTile);
    }

}