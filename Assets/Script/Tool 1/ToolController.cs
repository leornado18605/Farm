using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask soilLayer;
    [SerializeField] private float interactRange = 1.5f;

    [Header("Tools")]
    [SerializeField] private List<Tool> tools = new List<Tool>();
    [SerializeField] private int currentToolIndex = 0;
    
    private GroundTile targetTile;
    private Tool currentTool => tools.Count > 0 ? tools[currentToolIndex] : null;

    public void SelectTool(int index)
    {
        if (index < 0 || index >= tools.Count) return;
        currentToolIndex = index;
        Debug.Log($"🔧 Switched to: {tools[currentToolIndex].name}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TrySelectTile(out targetTile))
            {
                FaceToTile();
                if (targetTile.GetState() == SoilState.Normal && currentToolIndex != 0)
                {
                    currentToolIndex = 0;
                }
                currentTool?.Use(targetTile, player);
            }
        }
    }

    private bool TrySelectTile(out GroundTile tile)
    {
        Vector2 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(click, soilLayer);
        tile = hit ? hit.GetComponent<GroundTile>() : null;
        return tile != null;
    }

    private void FaceToTile()
    {
        Vector2 dir = (targetTile.transform.position - playerTransform.position).normalized;
        player.FaceDirection(dir);
    }

    public void OnHoeStage(int stage)
    {
        if (targetTile == null) return;
        GameEvents.RaiseHoeStage(stage, targetTile);
        if (stage == 3) 
        {
            NextTool();
            Debug.Log("🌾 Switched automatically to SeedTool after hoeing!");
        }
    }
    private void NextTool()
    {
        currentToolIndex++;
        if (currentToolIndex >= tools.Count)
            currentToolIndex = 0;
    }

    public void OnSeedStage(int stage)
    {
        if (targetTile == null) return;
        if (stage == 1)
        {
            targetTile.PlantSeed();
            player.StopSeeding();
        }
    }
    
    public void ShowBag(bool show)
    {
        player.ShowSeedBag(show);
    }

}
