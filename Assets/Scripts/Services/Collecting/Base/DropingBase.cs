using UnityEditor;
using UnityEngine;

public abstract class DropingBase<TItem,TZone> : MonoBehaviour
    where TItem : Object, new()
    where TZone : DropingZone<TItem>, new()
{
    private PickerBase<TItem> _picker;

    private void OnValidate()
    {
        if (TryGetComponent(out PickerBase<TItem> picker))
        {
            _picker = GetComponent<PickerBase<TItem>>();
        }
        else
        {
            EditorUtility.DisplayDialog(nameof(DropingBase<TItem, TZone>), $"Use it with {nameof(PickerBase<TItem>)}", "Понял");
        }
    }

    protected abstract bool CanDrop(TItem item);
    protected abstract void DropProcess(TItem item);

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TZone zoneToDrop))
        {
            var item = _picker.GetItem();

            if (CanDrop(item))
            {
                DropProcess(item);
                _picker.ClearItem();
                zoneToDrop.ProcessDrop(item);
            }
        }
    }
}