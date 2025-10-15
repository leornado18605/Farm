using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public enum SoilState
{
    Normal,    
    Hoeing,     
    Hoed,       
    Planted,
    Watered 
}

[RequireComponent(typeof(SpriteRenderer))]
public class GroundTile : MonoBehaviour
{
    public static readonly Dictionary<Collider2D, GroundTile> TileLookup = new();
    
    [Header("Sprites")]
    [SerializeField] private Sprite normalSoil;     
    [SerializeField] private Sprite hoeingSoil;     
    [SerializeField] private Sprite halfHoedSoil;  
    [SerializeField] private Sprite hoedSoil;      

    [Header("Effects")]
    [SerializeField] private GameObject dustEffectPrefab;
    [SerializeField] private AudioClip hoeSound;
    
    [Header("Plant Sprites")]
    [SerializeField] private Sprite sproutSprite;
    private GameObject sproutObject; 
    
    [Header("Water Sprites")]
    [SerializeField] private Sprite wateredSoil;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Collider2D myCollider;
    
    [SerializeField] private Crop cropPrefab;   // prefab cây
    [SerializeField] private CropData defaultCropData;
    
    private SoilState state = SoilState.Normal;
    public SpriteRenderer Renderer => spriteRenderer;

    //GetSet
    public Collider2D GetMyCollider => myCollider;
    
    void Awake()
    {
        spriteRenderer.sprite = normalSoil;
        if (myCollider != null && !TileLookup.ContainsKey(myCollider))
            TileLookup.Add(myCollider, this);
    }
    
    private void OnDestroy()
    {
        if (myCollider != null && TileLookup.ContainsKey(myCollider))
            TileLookup.Remove(myCollider);
    }
    private void HandleHoeStage(int stage, GroundTile tile)
    {
        if (tile != this) return; 

        switch (stage)
        {
            case 1: ChangeToStage1(); break;
            case 2: ChangeToStage2(); break;
            case 3: ChangeToStage3(); break;
        }
    }
    
    public void ChangeToStage1()
    {
        spriteRenderer.sprite = hoeingSoil;
        state = SoilState.Hoeing;
    }

    public void ChangeToStage2()
    {
        spriteRenderer.sprite = halfHoedSoil;
        state = SoilState.Hoeing;
    }

    public void ChangeToStage3()
    {
        spriteRenderer.sprite = hoedSoil;
        state = SoilState.Hoed;
    }

    public void PlantSeed()
    {
        if (state != SoilState.Hoed) return;

        state = SoilState.Planted;
        spriteRenderer.sprite = hoedSoil;

        Crop crop = CropPoolManager.Instance.SpawnCrop(this, defaultCropData);
        if (crop != null)
        {
            Vector3 tilePos = transform.position;
            float offsetY = 1f; // 👈 chỉnh nếu hạt bị lệch lên/xuống
            crop.transform.position = new Vector3(tilePos.x, tilePos.y + offsetY, tilePos.z);
            crop.SetSortingOrder(spriteRenderer.sortingOrder + 1);
        }
    }


    
    public SoilState GetState()
    {
        return state;
    }

    void OnEnable()
    {
        GameEvents.OnHoeStage += HandleHoeStage;
    }

    void OnDisable()
    {
        GameEvents.OnHoeStage -= HandleHoeStage;
    }

    public void WaterSoil()
    {
        if (state != SoilState.Planted)
        {
            return;
        }
        state = SoilState.Watered;
        spriteRenderer.sprite = wateredSoil;
        
    }
  
    public void SetState(SoilState newState)
    {
        state = newState;

        switch (state)
        {
            case SoilState.Normal:
                spriteRenderer.sprite = normalSoil;
                break;
            case SoilState.Hoeing:
                spriteRenderer.sprite = hoeingSoil;
                break;
            case SoilState.Hoed:
                spriteRenderer.sprite = hoedSoil;
                break;
            case SoilState.Planted:
                spriteRenderer.sprite = hoedSoil;
                break;
            case SoilState.Watered:
                spriteRenderer.sprite = wateredSoil;
                break;
        }

        Debug.Log($"🌾 Tile {name} đã chuyển sang trạng thái {state}");
    }
}
