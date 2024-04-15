using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public abstract class SpawnerBase<T> : MonoBehaviour
    where T : UnityEngine.Object
{
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _spawnPointsContainer;

    private ObjectPool<T> _pool;
    protected List<SpawnPoint> SpawnPoints { get; private set; }

    public event Action<T> NewSpawned;

    protected virtual void Awake()
    {
        SpawnPoints = new List<SpawnPoint>();

        foreach (SpawnPoint child in _spawnPointsContainer.GetComponentsInChildren<SpawnPoint>())
        {
            SpawnPoints.Add(child);
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
        if (IsAutoSpawn)
        {
            StartCoroutine(SpawnObjects());
        }
    }
   
    public void Release(T obj)
    {
        _pool.Release(obj);
    }

    protected virtual bool IsAutoSpawn { get; } = true;
    protected abstract SpawnPoint GetSpawnPoint();

    protected virtual bool CanSpawn()
    {
        return true;
    }

    protected virtual void ProcessGettingObject(T newObject, SpawnPoint spawnPosition)
    {
    }

    protected T SpawnNewObject()
    {
        if (CanSpawn())
        {
            return _pool.Get();
        }

        return null;
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

        if (spawnPoint != null)
        {
            spawnPoint.Free = false;

            this.ProcessGettingObject(obj, spawnPoint);

            gameObject.transform.position = spawnPoint.transform.position;
            gameObject.SetActive(true);

            NewSpawned?.Invoke(obj);
        }
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
            if (CanSpawn() && _pool.CountActive < SpawnPoints.Count)
            {
                _pool.Get();
            }

            yield return wait;
        }
    }
}