using UnityEngine;
using UnityEngine.Events;

public enum SoilState
{
    Normal,     // Chưa cuốc
    Hoeing,     // Đang cuốc
    Hoed,       // Đã cuốc xong hoàn toàn
    Planted     // Đã trồng cây
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

    [Header("Hoe Settings")]
    [SerializeField] private int totalHoeHits = 3;  

    [SerializeField] private SpriteRenderer spriteRenderer;
    private SoilState state = SoilState.Normal;
    private int hoeCount = 0;

    void Awake()
    {
        spriteRenderer.sprite = normalSoil;
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

}
