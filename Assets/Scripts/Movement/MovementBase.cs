using UnityEngine;

public abstract class MovementBase : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        if (CanMove())
        {
            PrepareMoveData();

            var nextPosition = GetNexPosition();

            transform.LookAt(nextPosition);
            transform.position = nextPosition;
        }
    }

    public abstract bool CanMove();

    public float GetSpeed()
    {
        return _speed;
    }

    protected abstract Vector3 GetNexPosition();

    protected virtual void PrepareMoveData()
    {
    }
}