using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Crystal : MonoBehaviour, IValueable
{
    [SerializeField] private int _amount;

    public int GetValue()
    {
        return _amount;
    }
}