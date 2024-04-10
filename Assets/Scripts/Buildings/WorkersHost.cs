using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

[RequireComponent(typeof(CrystalSearcher))]
[RequireComponent(typeof(CrystalCounter))]
[RequireComponent(typeof(WorkerSpawner))]
[RequireComponent(typeof(FlagPlacer))]

public class WorkersHost : MonoBehaviour
{
    const int WorkerCost = 30;
    const int HostCost = 50;

    private CrystalSearcher _crystalSearcher;
    private CrystalCounter _crystalCounter;
    private WorkerSpawner _workerSpawner;
    private FlagPlacer _flagPlacer;

    private static Queue<Crystal> _crystalsQueue;
    private List<int> _crystalsProcessed;
    private WorkersHostMode _mode;
    
    private void Awake()
    {
        _mode = WorkersHostMode.SpawnWorkers;
        _crystalsProcessed = new List<int>();
        _crystalSearcher = GetComponent<CrystalSearcher>();
        _workerSpawner = GetComponent<WorkerSpawner>();
        _crystalCounter = GetComponent<CrystalCounter>();
        _flagPlacer = GetComponent<FlagPlacer>();

        if (_crystalsQueue == null)
        {
            _crystalsQueue = new Queue<Crystal>();
        }
    }

    private void Start()
    {
        StartCoroutine(ProcessCrystalQueue());
    }

    private void OnEnable()
    {
        _crystalSearcher.ItemFound += QueueCrystalCollect;
        _crystalCounter.AmountUpdated += UseAmount;
    }

    private void OnDisable()
    {
        _crystalSearcher.ItemFound -= QueueCrystalCollect;
    }

    public void SetHostMode(WorkersHostMode mode)
    {
        _mode = mode;
    }

    public void PlaceFlag(Vector3 point)
    {
        _flagPlacer.Place(point, transform.rotation);
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

    private void QueueCrystalCollect(Crystal crystal)
    {
        var crystalHash = crystal.GetHashCode();

        if (!_crystalsProcessed.Contains(crystalHash))
        {
            _crystalsQueue.Enqueue(crystal);
            _crystalsProcessed.Add(crystalHash);
        }
    }

    private void UseAmount()
    {
        var currentAmount = _crystalCounter.Amount;

        if (_mode == WorkersHostMode.SpawnWorkers)
        {
            if (currentAmount >= WorkerCost)
            {
                _workerSpawner.SpawnNewWorker();
                _crystalCounter.DecreaseAmount(WorkerCost);
            }
        }
        else if(_mode == WorkersHostMode.BuildNewHost)
        {
            if (_flagPlacer.CurrentPosition.HasValue && currentAmount >= HostCost)
            {
                _crystalCounter.DecreaseAmount(HostCost);
                _workerSpawner.SendIdleWorkerForHostBuild(_flagPlacer.CurrentPosition.Value, BuildCallback);
                _mode = WorkersHostMode.SpawnWorkers;
            }
        }
    }

    private void BuildCallback()
    {
        _flagPlacer.ClearCurrent();
    }
}