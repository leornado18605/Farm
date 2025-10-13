using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Crop Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CropData cropData;

    private int currentStage = 0;
    private bool isGrowing = false;
    private float timer = 0f;

    public bool IsMature => currentStage >= cropData.growthSprites.Length - 1;

    // ⚡ Thay vì GetComponent, reference này được gán sẵn trong prefab
    public SpriteRenderer Renderer => spriteRenderer;

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
}