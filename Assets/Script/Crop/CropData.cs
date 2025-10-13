using UnityEngine;

[CreateAssetMenu(menuName = "Farming/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropName;
    public Sprite[] growthSprites;     
    public float timePerStage = 10f;   
}