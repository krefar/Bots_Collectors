using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpawner<T> : SpawnerBase<T>
    where T : Object, new()
{
    private List<Transform> _baseSpawnPoints;

    private new void Awake()
    {
        base.Awake();

        _baseSpawnPoints = GetSpawnPoints().ToList();
    }

    protected override Transform GetSpawnPoint()
    {
        if (_baseSpawnPoints.Count == 0)
        {
            _baseSpawnPoints = GetSpawnPoints().ToList();
        }

        var randomIndex = Random.Range(0, _baseSpawnPoints.Count);
        var randomPoint = _baseSpawnPoints[randomIndex];

        _baseSpawnPoints.RemoveAt(randomIndex);

        return randomPoint;
    }
}