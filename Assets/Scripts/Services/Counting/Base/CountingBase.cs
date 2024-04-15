using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class CountingBase<T> : MonoBehaviour
    where T : Object, IValuable, new() 
{
    [SerializeField] private DropingZone<T> _dropZone;
    [SerializeField] private int _amount;

    public abstract string Title { get; }
    
    public int Amount => _amount;
    public event Action AmountUpdated;

    private void OnEnable()
    {
        _dropZone.OnDrop += Increase;
    }

    private void OnDisable()
    {
        _dropZone.OnDrop -= Increase;
    }

    protected void Increase(T item)
    {
        _amount += item.GetValue();
        AmountUpdated?.Invoke();
    }

    protected void Increase(int amount)
    {
        _amount += amount;
        AmountUpdated?.Invoke();
    }

    protected void Decrease(int amount)
    {
        _amount -= amount;
        AmountUpdated?.Invoke();
    }
}