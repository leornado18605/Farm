using UnityEngine;

public abstract class Tool : ScriptableObject
{
    public abstract void Use(GroundTile tile, PlayerController player);
}