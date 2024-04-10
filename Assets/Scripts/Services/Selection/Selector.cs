using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Selector : MonoBehaviour
{
    private FlagPlacer _flagPlacer;
    private Outline _outline;

    private bool _selected;

    public bool Selected => _selected;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _flagPlacer = GetComponent<FlagPlacer>();
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