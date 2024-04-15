using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CrystalSearcher), typeof(CrystalCounter))]
[RequireComponent(typeof(WorkerSpawner), typeof(WorkerCommander))]
[RequireComponent(typeof(FlagPlacer))]
public class WorkersHost : MonoBehaviour
{
    const int WorkerCost = 30;
    const int HostCost = 50;

    private CrystalSearcher _crystalSearcher;
    private CrystalCounter _crystalCounter;
    private WorkerSpawner _workerSpawner;
    private WorkerCommander _workerCommander;
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
        _workerCommander = GetComponent<WorkerCommander>();
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

        if (_crystalCounter.Amount > 0)
        {
            UseAmount();
        }
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

    public void BindWorker(Worker worker)
    {
        _workerSpawner.BindWorker(worker);
        _workerCommander.AddWorker(worker);
    }
    
    public void UnbindWorker(Worker worker)
    {
        _workerSpawner.UnbindWorker(worker);
        _workerCommander.RemoveWorker(worker);
    }

    private IEnumerator ProcessCrystalQueue()
    {
        var wait = new WaitForSeconds(1);

        while (enabled) {

            if (_crystalsQueue.Count > 0)
            {
                if (_workerCommander.HasWorkers)
                {
                    var crystal = _crystalsQueue.Dequeue();
                    _workerCommander.SendIdleWorkerForCrystal(crystal);
                }
            }

            yield return wait;
        }
    }

    private void QueueCrystalCollect(Crystal crystal)
    {
        var crystalHash = crystal.GetHashCode();

        if (_crystalsProcessed.Contains(crystalHash) == false)
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
                var newWorker = _workerSpawner.SpawnNewWorker();
                if (newWorker != null)
                {
                    _workerCommander.AddWorker(newWorker);
                    _crystalCounter.DecreaseAmount(WorkerCost);
                }
            }
        }
        else if(_mode == WorkersHostMode.BuildNewHost)
        {
            if (_flagPlacer.CurrentPosition.HasValue && currentAmount >= HostCost)
            {
                _mode = WorkersHostMode.SpawnWorkers;
                _crystalCounter.DecreaseAmount(HostCost);
                _workerCommander.SendIdleWorkerForHostBuild(_flagPlacer.CurrentPosition.Value, UnbindWorker, BuildCallback);
            }
        }
    }

    private void BuildCallback()
    {
        _flagPlacer.ClearCurrent();
    }
}