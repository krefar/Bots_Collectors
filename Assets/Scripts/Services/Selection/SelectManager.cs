using UnityEngine;
using UnityEngine.InputSystem;

public class SelectManager : MonoBehaviour
{
    private const float RayDistance = 100;
    
    private WorkersHost _selectedWorkerHost;
    private Selector _selectedSelector;

    private bool _resetSelection;

    public void ProcessSelection()
    {
        var fromCameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        _resetSelection = true;

        if (Physics.Raycast(fromCameraRay, out RaycastHit hitInfo, RayDistance))
        {
            if (hitInfo.collider.TryGetComponent(out Selector selector))
            {
                ProcessSelector(selector);
            }
            else
            {
                var selectorInParent = hitInfo.collider.GetComponentInParent<Selector>(false);

                if (selectorInParent != null)
                {
                    ProcessSelector(selectorInParent);
                }
            }

            if (hitInfo.collider.TryGetComponent(out WorkersHost workersHost))
            {
                ProcessWorkerHost(workersHost);
            }
            else
            {
                var workerHostInParent = hitInfo.collider.GetComponentInParent<WorkersHost>(false);

                if (workerHostInParent != null)
                {
                    ProcessWorkerHost(workerHostInParent);
                }
            }
        }

        if (_resetSelection)
        {
            ResetSelection();
        }
    }

    public void ProcessActionWithSelected()
    {
        if (_selectedWorkerHost != null)
        {
            var fromCameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.value);

            if (Physics.Raycast(fromCameraRay, out RaycastHit hitInfo, RayDistance, 1))
            {
                if (hitInfo.collider.TryGetComponent(out FlagPlaceArea flagArea))
                {
                    _selectedWorkerHost.PlaceFlag(hitInfo.point);
                    _selectedWorkerHost.SetHostMode(WorkersHostMode.BuildNewHost);
                }
            }
        }
    }

    private void ProcessWorkerHost(WorkersHost workersHost)
    {
        _resetSelection = false;

        if (_selectedWorkerHost == null)
        {
            _selectedWorkerHost = workersHost;
        }
        else
        {
            _selectedWorkerHost = null;
        }
    }

    private void ProcessSelector(Selector selector)
    {
        _resetSelection = false;

        if (_selectedSelector != null && selector.gameObject.GetInstanceID() != _selectedSelector.gameObject.GetInstanceID())
        {
            _selectedSelector.Deselect();
            _selectedSelector = null;
        }

        if (selector.Selected)
        {
            selector.Deselect();
            _selectedSelector = null;
        }
        else
        {
            selector.Select();
            _selectedSelector = selector;
        }
    }

    private void ResetSelection()
    {
        if (_selectedSelector != null)
        {
            _selectedSelector.Deselect();
            _selectedSelector = null;
        }

        if (_selectedWorkerHost != null)
        {
            _selectedWorkerHost = null;
        }
    }
}