using Assets.Scripts.Extensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public abstract class SinglePlacer<T> : PlacerBase<T>
    where T : Object, new()
{
    private T _current;

    public Vector3? CurrentPosition => _current?.GameObject().transform.position;

    public override T Place(Vector3 position, Quaternion rotation)
    {
        if (_current == null)
        {
            _current = base.Place(position, rotation);
        }
        else
        {
            if (position.SamePosition(_current.GameObject().transform.position, Axis.X | Axis.Z))
            {
                ClearCurrent();
            }
            else
            {
                _current.GameObject().transform.position = position;
                _current.GameObject().transform.rotation = rotation;
            }
        }

        return _current;
    }

    public void ClearCurrent()
    {
        Destroy(_current.GameObject());
        _current = null;
    }
}