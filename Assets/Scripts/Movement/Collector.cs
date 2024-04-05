using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Collector : MovementBase
{
    private Queue<Vector3> _points;
    private Vector3 _startPoint;

    private Vector3 _nextPoint;
    private Vector3 _targetPosition;

    private bool _returned;

    public event Action StartPointReached;

    public void Init(Vector3 startPoint)
    {
        _startPoint = startPoint;
        _nextPoint = Vector3.zero;
        _points = new Queue<Vector3>();
    }

    public override bool CanMove()
    {
        if (transform.position.SamePosition(_startPoint, Axis.X | Axis.Z) && _points.Count == 0)
        {
            if (!_returned)
            {
                _returned = true;
                StartPointReached?.Invoke();
            }

            return false;
        }

        return true;
    }

    public void AddPoint(Vector3 wayPoint)
    {
        _points.Enqueue(wayPoint);
        _nextPoint = wayPoint;
    }

    public void Reset()
    {
        _points.Clear();
    }

    protected override void PrepareMoveData()
    {
        if (_nextPoint == Vector3.zero || transform.position.SamePosition(_nextPoint, Axis.X | Axis.Z))
        {
            if (_points.Count > 0)
            {
                _nextPoint = _points.Dequeue();
            }
            else
            {
                _nextPoint = Vector3.zero;
                _targetPosition = _startPoint;
            }
        }
        else
        {
            _targetPosition = _nextPoint;
        }

        _returned = false;
    }

    protected override Vector3 GetNexPosition()
    {
        var nextPosition = Vector3.MoveTowards(transform.position, _targetPosition, GetSpeed() * Time.deltaTime);
        nextPosition.y = transform.position.y;

        return nextPosition;
    }
}