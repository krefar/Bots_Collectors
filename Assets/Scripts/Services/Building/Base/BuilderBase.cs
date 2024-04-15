using System;
using System.Collections;
using UnityEngine;

public abstract class BuilderBase<T> : MonoBehaviour
    where T : UnityEngine.Object, new()
{
    [SerializeField] T _prefab;

    private ToPointMover _toPointMover;
    private Vector3 _point;

    bool _isBuildPointReached;

    public void Init(ToPointMover toPointMover)
    {
        _toPointMover = toPointMover;
    }

    public void GoToBuild(Vector3 point, Action callback = null)
    {
        _point = point;
        _toPointMover.Init(_point);
        _toPointMover.PointReached += SetBuildPointReached;

        StartCoroutine(MoveAndBuild(callback));
    }

    private void SetBuildPointReached()
    {
        _isBuildPointReached = true;
    }

    private IEnumerator MoveAndBuild(Action callback = null)
    {
        yield return new WaitUntil(() => _isBuildPointReached);

        _toPointMover.PointReached -= SetBuildPointReached;
        _isBuildPointReached = false;

        var building = Instantiate(_prefab, transform.position, new Quaternion());

        UpdateBuildingProps(building);

        if (callback != null)
        {
            callback();
        }
    }

    protected virtual void UpdateBuildingProps(T building)
    {
    }
}