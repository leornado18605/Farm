using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Seed Tool")]
public class SeedTool : Tool
{
    public override void Use(GroundTile tile, PlayerController player)
    {
        if (tile == null)
        {
            return;
        }

        if (tile.GetState() != SoilState.Hoed)
        {
            return;
        }

        player.StartSeeding();
    }
}