using UnityEngine;

[RequireComponent(typeof(Outline))]
public class SelectorTarget : MonoBehaviour
{
    private Outline _outline;

    private bool _selected;

    public bool Selected => _selected;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void Select()
    {
        _selected = true;
        _outline.OutlineWidth = 5;
    }

    public void Deselect()
    {
        _selected = false;
        _outline.OutlineWidth = 0;
    }
}