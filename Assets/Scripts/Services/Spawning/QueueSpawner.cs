using System.Linq;

public abstract class QueueSpawner<T> : SpawnerBase<Worker>
    where T : class, new()
{
    protected override SpawnPoint GetSpawnPoint()
    {
        var spawnPoint = SpawnPoints.FirstOrDefault( p => p.Free);

        return spawnPoint;
    }
}