using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private int hoeCount = 0;

    void OnEnable() => GameEvents.OnHoeStage += UpdateQuest;
    void OnDisable() => GameEvents.OnHoeStage -= UpdateQuest;

    private void UpdateQuest(int stage, GroundTile tile)
    {
        if (stage == 3) 
        {
            hoeCount++;
        }
    }
}