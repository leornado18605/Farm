using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Hoe Tool")]
public class HoeTool : Tool
{
    public override void Use(GroundTile tile, PlayerController player)
    {
        if (tile == null) return;
        if (tile.GetState() == SoilState.Hoed || tile.GetState() == SoilState.Planted)
            return;

        player.StartHoeing(); 
    }
}