using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class DropingZone<T> : MonoBehaviour
    where T : Object, new()
{
    public event Action<T> OnDrop;

    public void ProcessDrop(T item)
    {
        OnDrop?.Invoke(item);
    }
}