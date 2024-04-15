using System;
using UnityEngine;

[RequireComponent (typeof(Collector))]
[RequireComponent(typeof(ToPointMover))]
[RequireComponent (typeof(WorkersHostBuilder))]
public class Worker : MonoBehaviour, IEquatable<Worker>

{

    private Collector _collector;
    private ToPointMover _toPointMover;
    private WorkersHostBuilder _workersHostBuilder;

    private bool _isIdle;

    public bool IsIdle => _isIdle;

    private void Awake()
    {
        _isIdle = true;
        _collector = GetComponent<Collector>();
        _toPointMover = GetComponent<ToPointMover>();
        _toPointMover.enabled = false;

        _workersHostBuilder = GetComponent<WorkersHostBuilder>();
        _workersHostBuilder.Init(_toPointMover);
    }

    public void GoForCrystal(Crystal crystal)
    {
        if (_isIdle)
        {
            _isIdle = false;
            _collector.AddPoint(crystal.transform.position);
            _collector.StartPointReached += SetIdle;
        }
    }

    public void GoForHostBuild(Vector3 point, Action callback = null)
    {
        if (_isIdle)
        {
            _isIdle = false;
            _collector.enabled = false;
            _toPointMover.enabled = true;
            _toPointMover.PointReached += SetIdle;

            _workersHostBuilder.GoToBuild(point, callback);
        }
    }

    public bool Equals(Worker other)
    {
        return this.GetInstanceID() == other.GetInstanceID();
    }

    private void SetIdle()
    {
        _isIdle = true;
        _collector.enabled = true;
        _toPointMover.enabled = false;
        _collector.StartPointReached -= SetIdle;
    }
}