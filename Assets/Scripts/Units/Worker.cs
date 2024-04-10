using System;
using UnityEngine;

[RequireComponent (typeof(Collector))]
[RequireComponent(typeof(ToPointMover))]
[RequireComponent (typeof(WorkersHostBuilder))]
public class Worker : MonoBehaviour
{

    private Collector _collector;
    private ToPointMover _toPointMover;
    private WorkersHostBuilder _workersHostBuilder;

    private bool _idle;

    public bool Idle => _idle;

    private void Awake()
    {
        _idle = true;
        _collector = GetComponent<Collector>();
        _toPointMover = GetComponent<ToPointMover>();
        _toPointMover.enabled = false;

        _workersHostBuilder = GetComponent<WorkersHostBuilder>();
        _workersHostBuilder.Init(_toPointMover);
    }

    public void GoForCrystal(Crystal crystal)
    {
        if (_idle)
        {
            _idle = false;
            _collector.AddPoint(crystal.transform.position);
            _collector.StartPointReached += SetIdle;
        }
    }

    public void GoForHostBuild(Vector3 point, Action callback = null)
    {
        if (_idle)
        {
            _idle = false;
            _collector.enabled = false;
            _toPointMover.enabled = true;
            _toPointMover.PointReached += SetIdle;

            _workersHostBuilder.Build(point, callback);
        }
    }

    private void SetIdle()
    {
        _idle = true;
        _collector.enabled = true;
        _toPointMover.enabled = false;
        _collector.StartPointReached -= SetIdle;
    }
}