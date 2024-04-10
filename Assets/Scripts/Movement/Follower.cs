using UnityEngine;

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