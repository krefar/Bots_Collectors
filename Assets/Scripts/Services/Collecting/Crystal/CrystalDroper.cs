using Unity.VisualScripting;
using UnityEditor.Sprites;
using UnityEngine;

public class CrystalDroper : DropingBase<Crystal, CrystalDropingZone>
{
    protected override bool CanDrop(Crystal item)
    {
        return item != null;
    }

    protected override void DropProcess(Crystal item)
    {
        item.gameObject.SetActive(false);
    }
}