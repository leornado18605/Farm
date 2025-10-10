using UnityEngine;
using UnityEngine.Events;

public enum SoilState
{
    Normal,    
    Hoeing,     
    Hoed,       
    Planted    
}

[RequireComponent(typeof(SpriteRenderer))]
public class GroundTile : MonoBehaviour
{
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
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    private SoilState state = SoilState.Normal;
    void Awake()
    {
        spriteRenderer.sprite = normalSoil;
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

        if (sproutObject != null)
            Destroy(sproutObject);

        sproutObject = new GameObject("Sprout");
        var sproutSR = sproutObject.AddComponent<SpriteRenderer>();
        sproutSR.sprite = sproutSprite;
        sproutSR.sortingOrder = spriteRenderer.sortingOrder + 1; 
        sproutSR.transform.SetParent(transform);
        sproutSR.transform.localPosition = new Vector3(0, 0.05f, 0); 

        Debug.Log("🌱 Seed planted, sprout displayed!");
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

   

}
