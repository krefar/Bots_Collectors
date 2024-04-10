using UnityEngine;

public class SpawnPoint
{
    public Transform Transform { get; set; }
    public bool Free { get; set; }

    public SpawnPoint(Transform transform)
    {
        Transform = transform;
        Free = true;
    }
}