using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public abstract class SpawnerBase<T> : MonoBehaviour
    where T : Object
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _spawnPointsContainer;

    private ObjectPool<T> _pool;
    protected List<SpawnPoint> SpawnPoints { get; private set; }
    protected Queue<SpawnPoint> DisabledPoints { get; private set; }

    protected virtual void Awake()
    {
        SpawnPoints = new List<SpawnPoint>();
        DisabledPoints = new Queue<SpawnPoint>();

        foreach (Transform child in _spawnPointsContainer)
        {
            if (child.gameObject.activeInHierarchy)
            {
                SpawnPoints.Add(new SpawnPoint(child));
            }
            else
            {
                DisabledPoints.Enqueue(new SpawnPoint(child));
            }
        }

        _pool = new ObjectPool<T>(
            createFunc: () => CreateObject(),
            actionOnGet: (obj) => GetObject(obj),
            actionOnRelease: (obj) => ReleaseObject(obj),
            actionOnDestroy: (obj) => DestroyObject(obj),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 20
            );
    }

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    public void Release(T obj)
    {
        _pool.Release(obj);
    }

    protected abstract SpawnPoint GetSpawnPoint();
    protected virtual void ProcessGettingObject(T newObject, Vector3 spawnPosition)
    {
    }

    protected bool AddSpawnPointFromDisabled()
    {
        if (DisabledPoints.Any())
        {
            var newSpawnPoint = DisabledPoints.Dequeue();
            newSpawnPoint.Transform.gameObject.SetActive(true);

            SpawnPoints.Add(newSpawnPoint);

            return true;
        }

        return false;
    }

    private void DestroyObject(T obj)
    {
        var gameObject = obj.GameObject();

        Destroy(gameObject);
    }

    private void GetObject(T obj)
    {
        var gameObject = obj.GameObject();
        var spawnPoint = GetSpawnPoint();
        spawnPoint.Free = false;

        this.ProcessGettingObject(obj, spawnPoint.Transform.position);

        gameObject.transform.position = spawnPoint.Transform.position;
        gameObject.SetActive(true);
    }

    private void ReleaseObject(T obj)
    {
        var gameObject = obj.GameObject();

        gameObject.SetActive(false);
    }

    private T CreateObject()
    {
        return (T)PrefabUtility.InstantiatePrefab(_prefab); 
    }

    private IEnumerator SpawnObjects()
    {
        var wait = new WaitForSeconds(1);

        while (enabled)
        {
            if (_pool.CountActive < SpawnPoints.Count)
            {
                _pool.Get();
            }

            yield return wait;
        }
    }
}