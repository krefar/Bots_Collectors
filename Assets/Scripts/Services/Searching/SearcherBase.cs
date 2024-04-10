using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class SearcherBase<T> : MonoBehaviour
{
    [SerializeField] private float _searchRadius;
    [SerializeField] private float _searchDelay;
    [SerializeField] private ParticleSystem _scanEffect;
    [SerializeField] private Vector3 _scanEffectPointOffset;

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
        var hitVFX = Instantiate(_scanEffect, transform.position + _scanEffectPointOffset, transform.rotation);

        Destroy(hitVFX.gameObject, hitVFX.main.duration);
    }

    private void SearchAround()
    {
        var results = new Collider[50];
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