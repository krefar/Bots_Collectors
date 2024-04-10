using Assets.Scripts.Extensions;
using System;
using UnityEngine;
using UnityEngine.Animations;

public class ToPointMover : MovementBase
{
    private Vector3 _targetPoint;
    private bool _moveFinished;

    public event Action PointReached;

    public void Init(Vector3 targetPoint)
    {
        _targetPoint = targetPoint;
    }

    public override bool CanMove()
    {
        var pointReached = transform.position.SamePosition(_targetPoint, Axis.X | Axis.Z);

        if (!_moveFinished && pointReached)
        {
            PointReached?.Invoke();
            _moveFinished = true;
        }

        return !pointReached;
    }

    protected override Vector3 GetNexPosition()
    {
        var nextPosition = Vector3.MoveTowards(transform.position, _targetPoint, Time.deltaTime * GetSpeed());
        nextPosition.y = transform.position.y;

        return nextPosition;
    }
}