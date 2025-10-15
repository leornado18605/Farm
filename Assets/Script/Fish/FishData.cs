using UnityEngine;

[CreateAssetMenu(menuName = "Fishing/Fish Data")]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite fishSprite;
    public int minValue;
    public int maxValue;
    [Range(0, 1)] public float catchChance = 0.5f; // Xác suất bắt được
    public float minBiteTime = 1.5f;
    public float maxBiteTime = 4.0f;
}
