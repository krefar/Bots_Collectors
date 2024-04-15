using UnityEngine;
using UnityEngine.InputSystem;

public class Selector : MonoBehaviour
{
    private const float RayDistance = 100;

    private WorkersHost _selectedWorkerHost;
    private SelectorTarget _selectedTarget;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void ProcessSelection()
    {
        var fromCameraRay = _camera.ScreenPointToRay(Mouse.current.position.value);
        var selectionLayerMask = 991;

        if (Physics.Raycast(fromCameraRay, out RaycastHit hitInfo, RayDistance, selectionLayerMask))
        {
            ProcessSelectorTarget(hitInfo);
            ProcessWorkerHost(hitInfo);
        }
    }

    public void ProcessActionWithSelected()
    {
        if (_selectedWorkerHost != null)
        {
            var fromCameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.value);
            var defaultLayerMask = 1;

            if (Physics.Raycast(fromCameraRay, out RaycastHit hitInfo, RayDistance, defaultLayerMask))
            {
                if (hitInfo.collider.TryGetComponent(out FlagPlaceArea flagArea))
                {
                    _selectedWorkerHost.PlaceFlag(hitInfo.point);
                    _selectedWorkerHost.SetHostMode(WorkersHostMode.BuildNewHost);
                }
            }
        }
    }

    private void ProcessWorkerHost(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent(out WorkersHost workersHost))
        {
            if (_selectedWorkerHost == null)
            {
                _selectedWorkerHost = workersHost;
            }
            else
            {
                _selectedWorkerHost = null;
            }
        }
        else
        {
            _selectedWorkerHost = null;
        }
    }

    private void ProcessSelectorTarget(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent(out SelectorTarget target))
        {
            if (_selectedTarget != null && target.gameObject.GetInstanceID() != _selectedTarget.gameObject.GetInstanceID())
            {
                _selectedTarget.Deselect();
                _selectedTarget = null;
            }

            if (target.Selected)
            {
                target.Deselect();
                _selectedTarget = null;
            }
            else
            {
                target.Select();
                _selectedTarget = target;
            }
        }
        else
        {
            if (_selectedTarget != null)
            {
                _selectedTarget.Deselect();
                _selectedTarget = null;
            }
        }
    }
}