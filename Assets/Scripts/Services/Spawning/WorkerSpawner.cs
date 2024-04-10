using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkerSpawner : QueueSpawner<Worker>
{
    private List<Worker> _workers;

    protected override void Awake()
    {
        base.Awake();

        _workers = new List<Worker>();
    }

    public void SendIdleWorkerForCrystal(Crystal crystal)
    {
        StartCoroutine(SendIdleWorkerForCrystalInternal(crystal));
    }

    public void SendIdleWorkerForHostBuild(Vector3 point, Action callback = null)
    {
        StartCoroutine(SendIdleWorkerForHostBuildInternal(point, callback));
    }

    public void SpawnNewWorker()
    {
        var result = AddSpawnPointFromDisabled();
    }

    protected override void ProcessGettingObject(Worker newObject, Vector3 spawnPosition)
    {
        _workers.Add(newObject);

        base.ProcessGettingObject(newObject, spawnPosition);

        var collector = newObject.GetComponent<Collector>();
        collector.Init(spawnPosition);
    }

    private IEnumerator SendIdleWorkerForCrystalInternal(Crystal crystal)
    {
        var wait = new WaitForSeconds(1);

        while (enabled)
        {
            var idleWorker = _workers.FirstOrDefault(w => w.Idle);

            if (idleWorker != null)
            {
                idleWorker.GoForCrystal(crystal);
                yield break;
            }

            yield return wait;
        }
    }

    private IEnumerator SendIdleWorkerForHostBuildInternal(Vector3 point, Action callback = null)
    {
        var wait = new WaitForSeconds(1);

        while (enabled)
        {
            var idleWorker = _workers.FirstOrDefault(w => w.Idle);

            if (idleWorker != null)
            {
                idleWorker.GoForHostBuild(point, callback);
                yield break;
            }

            yield return wait;
        }
    }
}