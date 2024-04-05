using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrystalSearcher))]
[RequireComponent(typeof(WorkerSpawner))]

public class WorkersHost : MonoBehaviour
{
    private CrystalSearcher _crystalSearcher;
    private WorkerSpawner _workerSpawner;

    private Queue<Crystal> _crystalsQueue;
    private List<int> _crystalsProcessed;

    private void Awake()
    {
        _crystalsQueue = new Queue<Crystal>();
        _crystalsProcessed = new List<int>();
        _crystalSearcher = GetComponent<CrystalSearcher>();
        _workerSpawner = GetComponent<WorkerSpawner>();
    }

    private void Start()
    {
        StartCoroutine(ProcessCrystalQueue());
    }

    private IEnumerator ProcessCrystalQueue()
    {
        var wait = new WaitForSeconds(1);

        while (enabled) {

            if (_crystalsQueue.Count > 0)
            {
                var crystal = _crystalsQueue.Dequeue();
                _workerSpawner.SendIdleWorkerForCrystal(crystal);
            }

            yield return wait;
        }
    }

    private void OnEnable()
    {
        _crystalSearcher.ItemFound += QueueCrystalCollect;
    }

    private void OnDisable()
    {
        _crystalSearcher.ItemFound -= QueueCrystalCollect;
    }

    private void QueueCrystalCollect(Crystal crystal)
    {
        var crystalHash = crystal.GetHashCode();

        if (!_crystalsProcessed.Contains(crystalHash))
        {
            _crystalsQueue.Enqueue(crystal);
            _crystalsProcessed.Add(crystalHash);
        }
    }
}