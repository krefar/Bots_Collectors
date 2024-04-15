using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, IEquatable<SpawnPoint>
{
    public bool Free { get; set; }

    public SpawnPoint()
    {
        Free = true;
    }

    public bool Equals(SpawnPoint other)
    {
        return this.GetInstanceID() == other.GetInstanceID();
    }
}