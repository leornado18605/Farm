using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Axe Tool")]
public class AxeTool : Tool
{
    [Header("Axe Settings")]
    [SerializeField] private float harvestRange = 1.2f;
    private Crop targetCrop;

    public override void Use(GroundTile tile, PlayerController player)
    {
        // Lấy cây ở tile
        Crop crop = CropPoolManager.Instance.GetCropAtTile(tile);
        if (crop == null || !crop.IsMature)
        {
            Debug.Log("🌱 Không có cây chín ở ô này!");
            return;
        }

        targetCrop = crop; // lưu lại cây để chặt sau

        float dist = Vector2.Distance(player.transform.position, tile.transform.position);
        if (dist > harvestRange)
            player.MoveToAndAct(tile.transform.position, () => StartSwing(player));
        else
            StartSwing(player);
    }

    private void StartSwing(PlayerController player)
    {
        player.FaceDirection((targetCrop.transform.position - player.transform.position).normalized);
        player.StartHarvestAnim(targetCrop);
    }
}