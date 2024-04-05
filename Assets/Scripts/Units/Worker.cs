using UnityEngine;

[RequireComponent (typeof(Collector))]
public class Worker : MonoBehaviour
{
    private Collector _collector;

    [SerializeField] private bool _idle;

    public bool Idle => _idle;

    private void Awake()
    {
        _idle = true;
        _collector = GetComponent<Collector>();
    }

    public void GoForCrystal(Crystal crystal)
    {
        if (_idle)
        {
            _idle = false;
            _collector.AddPoint(crystal.transform.position);
            _collector.StartPointReached += SetIdle;
        }
    }

    private void SetIdle()
    {
        _idle = true;
        _collector.StartPointReached -= SetIdle;
    }
}