using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Crystal : MonoBehaviour, IValuable
{
    [SerializeField] private int _amount;

    public int GetValue()
    {
        return _amount;
    }
}