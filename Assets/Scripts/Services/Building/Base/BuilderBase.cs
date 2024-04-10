using System;
using System.Collections;
using UnityEngine;

public abstract class BuilderBase<T> : MonoBehaviour
    where T : UnityEngine.Object, new()
{
    [SerializeField] T _prefab;

    private ToPointMover _toPointMover;
    private Vector3 _point;

    bool _processBuild;

    public void Init(ToPointMover toPointMover)
    {
        _toPointMover = toPointMover;
    }

    public void Build(Vector3 point, Action callback = null)
    {
        _point = point;
        _toPointMover.Init(_point);
        _toPointMover.PointReached += SetBuildProcess;

        StartCoroutine(MoveAndBuild(callback));
    }

    private void SetBuildProcess()
    {
        _processBuild = true;
    }

    private IEnumerator MoveAndBuild(Action callback = null)
    {
        yield return new WaitUntil(() => _processBuild);

        _toPointMover.PointReached -= SetBuildProcess;
        _processBuild = false;

        Instantiate(_prefab, transform.position, new Quaternion());

        if (callback != null)
        {
            callback();
        }
    }
}