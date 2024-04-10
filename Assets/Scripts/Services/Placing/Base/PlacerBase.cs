using UnityEngine;

public abstract class PlacerBase<T> : MonoBehaviour
    where T : Object, new()
{
    [SerializeField] private T _prefab;

    public virtual T Place(Vector3 position, Quaternion rotation)
    {
        return Instantiate(_prefab, position, rotation);
    }
}