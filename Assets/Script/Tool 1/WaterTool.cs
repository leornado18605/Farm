using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Water Tool")]
public class WaterTool : Tool
{
    public override void Use(GroundTile tile, PlayerController player)
    {
        if (tile == null)
            return;

        if (tile.GetState() != SoilState.Planted)
            return;

        player.StartWatering();
    }
}