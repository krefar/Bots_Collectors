using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpawner<T> : SpawnerBase<T>
    where T : Object, new()
{
    protected override SpawnPoint GetSpawnPoint()
    {
        var freeSpawnPoint = SpawnPoints.Where(p => p.Free).ToList();
        var freeSpawnPointCount = freeSpawnPoint.Count;

        if (freeSpawnPointCount > 0)
        {
            var randomIndex = Random.Range(0, freeSpawnPointCount);
            var randomPoint = freeSpawnPoint[randomIndex];

            return randomPoint;
        }

        return null;
    }
}