using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkerCommander : MonoBehaviour
{
    private List<Worker> _workers;

    public bool HasWorkers => _workers.Count > 0;

    private void Awake()
    {
        _workers = new List<Worker>();
    }

    public void AddWorker(Worker worker)
    {
        _workers.Add(worker);
    }

    public void RemoveWorker(Worker worker)
    {
        _workers.Remove(worker);
    }

    public void SendIdleWorkerForCrystal(Crystal crystal)
    {
        StartCoroutine(SendIdleWorkerForCrystalInternal(crystal));
    }

    public void SendIdleWorkerForHostBuild(Vector3 point, Action<Worker> actionWithWorker, Action callback = null)
    {
        StartCoroutine(SendIdleWorkerForHostBuildInternal(point, actionWithWorker, callback));
    }

    private IEnumerator SendIdleWorkerForCrystalInternal(Crystal crystal)
    {
        var wait = new WaitForSeconds(1);

        while (enabled)
        {
            var idleWorker = _workers.FirstOrDefault(w => w.IsIdle);

            if (idleWorker != null)
            {
                idleWorker.GoForCrystal(crystal);
                yield break;
            }

            yield return wait;
        }
    }

    private IEnumerator SendIdleWorkerForHostBuildInternal(Vector3 point, Action<Worker> actionWithWorker, Action callback = null)
    {
        var wait = new WaitForSeconds(1);

        while (enabled)
        {
            var idleWorker = _workers.FirstOrDefault(w => w.IsIdle);

            if (idleWorker != null)
            {
                idleWorker.GoForHostBuild(point, callback);
                actionWithWorker(idleWorker);
                yield break;
            }

            yield return wait;
        }
    }
}