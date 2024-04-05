using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;

public class Follower : MovementBase
{
    private Transform _target;
    private Vector3 _offset;

    public void Init(Transform target, Vector3 offset)
    {
        _target = target;
        _offset = offset;
    }

    public override bool CanMove()
    {
        return true;
    }

    protected override Vector3 GetNexPosition()
    {
        return _target.position + _offset;
    }
}