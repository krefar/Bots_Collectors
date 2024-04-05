using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class SearcherBase<T> : MonoBehaviour
{
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _searchDelay;
    [SerializeField] private ParticleSystem _scanEffect;

    public event Action<T> ItemFound;

    private void Start()
    {
        StartCoroutine(SearchItems());
    }

    private IEnumerator SearchItems()
    {
        var wait = new WaitForSeconds(_searchDelay);

        while (enabled)
        {
            if (_scanEffect != null)
            {
                InstantiateScanEffect();
            }

            SearchAround();

            yield return wait;
        }
    }

    private void InstantiateScanEffect()
    {
        var hitVFX = Instantiate(_scanEffect, transform.position, Quaternion.Euler(0, 0, 0));

        Destroy(hitVFX.gameObject, hitVFX.main.duration);
    }

    private void SearchAround()
    {
        var results = new Collider[10];
        var colliders = Physics.OverlapSphereNonAlloc(transform.position, _searchRadius, results);

        foreach (var collider in results)
        {
            if (collider != null && collider.TryGetComponent(out T item))
            {
                ItemFound?.Invoke(item);
            }
        }
    }
}