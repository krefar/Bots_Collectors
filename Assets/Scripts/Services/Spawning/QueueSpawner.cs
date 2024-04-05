using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class QueueSpawner<T> : SpawnerBase<Worker>
    where T : class, new()
{
    private Queue<Transform> _spawnPointsQueue;

    protected override void Awake()
    {
        base.Awake();

        _spawnPointsQueue = new Queue<Transform>(GetSpawnPoints());
    }

    protected override Transform GetSpawnPoint()
    {
        var spawnPoint = _spawnPointsQueue.Dequeue();

        _spawnPointsQueue.Enqueue(spawnPoint);

        return spawnPoint;
    }
}