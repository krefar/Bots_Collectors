using Unity.VisualScripting;
using UnityEngine;

public class CrystalPicker : PickerBase<Crystal>
{
    private Crystal _crystal;

    public override void ClearItem()
    {
        _crystal = null;
    }

    public override Crystal GetItem()
    {
        return _crystal;
    }

    protected override bool CanPick(Crystal item)
    {
        return _crystal == null;
    }

    protected override void PickProcess(Crystal item)
    {
        _crystal = item;

        var itemFollower = item.AddComponent<Follower>();
        itemFollower.Init(transform, new Vector3(0f, 0.5f, 0));
    }
}