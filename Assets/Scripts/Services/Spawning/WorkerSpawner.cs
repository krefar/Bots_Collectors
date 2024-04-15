using System.Collections.Generic;
using UnityEngine;

public class WorkerSpawner : FreePointSpawner<Worker>
{
    [SerializeField] private Dictionary<Worker, SpawnPoint> _workers;

    protected override bool IsAutoSpawn => false;

    protected override void Awake()
    {
        base.Awake();

        _workers = new Dictionary<Worker, SpawnPoint>();
    }

    public void BindWorker(Worker worker)
    {
        var collector = worker.GetComponent<Collector>();
        var spawnPoint = GetSpawnPoint();

        if (spawnPoint != null)
        {
            spawnPoint.Free = false;
            collector.Init(spawnPoint.transform.position);
            _workers.Add(worker, spawnPoint);
        }
    }
    
    public void UnbindWorker(Worker worker)
    {
        _workers[worker].Free = true;
        _workers.Remove(worker);
    }

    public Worker SpawnNewWorker()
    {
        return SpawnNewObject();
    }

    protected override bool CanSpawn()
    {
        return _workers.Count < SpawnPoints.Count;
    }

    protected override void ProcessGettingObject(Worker newObject, SpawnPoint spawnPoint)
    {
        _workers.Add(newObject, spawnPoint);

        base.ProcessGettingObject(newObject, spawnPoint);

        var collector = newObject.GetComponent<Collector>();
        collector.Init(spawnPoint.transform.position);
    }
}