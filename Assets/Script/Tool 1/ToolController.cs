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

    private GroundTile targetTile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(click);

            if (hit == null) return;

            HandleClick(hit, click);
        }
    }
    private void HandleClick(Collider2D hit, Vector2 click)
    {
        Crop crop = CropPoolManager.Instance.GetCropByCollider(hit);
        if (crop != null)
        {
            // Nếu cây đã chín nhưng chưa bị chặt => chặt
            if (crop.IsMature && !crop.IsCutDown)
            {
                AxeTool axe = FindTool<AxeTool>();
                if (axe != null)
                    axe.Use(crop.GetBoundTile(), player);
                return;
            }

            // Nếu cây đã bị chặt nằm xuống => click để xóa
            if (crop.IsCutDown)
            {
                crop.TryRemoveAfterCut();
                return;
            }
        }
        
        // 🎣 Nếu click vào vùng nước
        if (hit.CompareTag("Water"))
        {
            HandleFishingClick(click);
            return;
        }

        // 🌱 Nếu click vào đất
        if (TrySelectTile(out targetTile))
        {
            HandleSoilClick(targetTile);
        }
    }

// 🎣 Fishing Handler
    private void HandleFishingClick(Vector2 click)
    {
        FishingRodTool fishingRod = FindTool<FishingRodTool>();
        if (fishingRod == null)
        {
            Debug.LogWarning("❌ Không tìm thấy FishingRodTool trong danh sách Tools!");
            return;
        }

        float dist = Vector2.Distance(playerTransform.position, click);
        if (dist > interactRange)
            player.MoveToAndAct(click, () => fishingRod.TryFish(click, player));
        else
            fishingRod.TryFish(click, player);
    }

// 🌱 Soil Handler
    private void HandleSoilClick(GroundTile tile)
    {
        Tool autoTool = DetectToolForTile(tile);
        if (autoTool == null)
        {
            Debug.Log("⚠️ Không có tool phù hợp với trạng thái đất hiện tại!");
            return;
        }

        autoTool.Use(tile, player);
    }
    private bool TrySelectTile(out GroundTile tile)
    {
        tile = GetClickedTile();
        if (tile == null) return false;

        if (IsTileTooFar(tile))
        {
            MoveToTileAndUse(tile);
            return false;
        }

        return true;
    }

    private GroundTile GetClickedTile()
    {
        Vector2 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(click, soilLayer);

        if (hit == null) return null;

        if (GroundTile.TileLookup.TryGetValue(hit, out GroundTile tile))
            return tile;

        return null;
    }

    private bool IsTileTooFar(GroundTile tile)
    {
        float dist = Vector2.Distance(playerTransform.position, tile.transform.position);
        return dist > interactRange;
    }

    private void MoveToTileAndUse(GroundTile tile)
    {
        Vector2 targetPos = tile.transform.position;
        Vector2 dir = (targetPos - (Vector2)playerTransform.position).normalized;
        Vector2 stopPos = targetPos - dir * 0.5f;

        player.MoveToAndAct(stopPos, () =>
        {
            player.FaceDirection(dir);
            Tool autoTool = DetectToolForTile(tile);
            autoTool?.Use(tile, player);
        });
    }

    private void FaceToTile()
    {
        Vector2 dir = (targetTile.transform.position - playerTransform.position).normalized;
        player.FaceDirection(dir);
    }

    private Tool DetectToolForTile(GroundTile tile)
    {
        switch (tile.GetState())
        {
            case SoilState.Normal:
            case SoilState.Hoeing:
                return FindToolByName("Hoe");
            case SoilState.Hoed:
                return FindToolByName("Seed");
            case SoilState.Planted:
                return FindTool<WaterTool>();
            default:
                return null;
        }
    }

    private T FindTool<T>() where T : Tool
    {
        foreach (var t in tools)
            if (t is T tool) return tool;
        return null;
    }

    private Tool FindToolByName(string keyword)
    {
        foreach (var t in tools)
            if (t.name.Contains(keyword)) return t;
        return null;
    }

    public void OnHoeStage(int stage)
    {
        if (targetTile == null) return;
        GameEvents.RaiseHoeStage(stage, targetTile);
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

    public void OnWaterStage(int stage)
    {
        if (targetTile == null) return;
        if (stage == 2)
        {
            targetTile.WaterSoil();
            player.StopWatering();
        }
    }

    public void ShowBag(bool show)
    {
        player.ShowSeedBag(show);
    }
}
