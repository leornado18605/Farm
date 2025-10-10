using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<int, GroundTile> OnHoeStage;

    public static void RaiseHoeStage(int stage, GroundTile tile)
    {
        OnHoeStage?.Invoke(stage, tile);
    }
}