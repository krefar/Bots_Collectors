using UnityEngine;

public abstract class PickerBase<T> : MonoBehaviour
    where T : Object, new()
{
    public abstract T GetItem();
    public abstract void ClearItem();

    protected abstract bool CanPick(T item);
    protected abstract void PickProcess(T item);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out T itemToPick))
        {
            if (CanPick(itemToPick))
            {
                PickProcess(itemToPick);
            }
        }
    }
}