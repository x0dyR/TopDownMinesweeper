using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Bomb : MonoBehaviour
{
    public event Action Exploded;
    public event Action StartedExplosion;
    public event Action StopedExplosion;

    [SerializeField] private BombView _view;

    [SerializeField] private float _timeToExplode;
    [SerializeField] private int _damage;

    [SerializeField] private SphereCollider _collider;

    private Collider[] _overlapedColliders;

    private Coroutine _explodeCoroutine;

    public float TimeToExplode => _timeToExplode;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _overlapedColliders = new Collider[32];
        _view.Initialize(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable _))
        {
            StartedExplosion?.Invoke();
            _explodeCoroutine ??= StartCoroutine(ExplodeTimer(_timeToExplode));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopedExplosion?.Invoke();

        StopCoroutine(_explodeCoroutine);
        _explodeCoroutine = null;
    }


    private IEnumerator ExplodeTimer(float timeToDetonate)
    {
        yield return new WaitForSeconds(timeToDetonate);

        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _collider.bounds.extents.z, _overlapedColliders);

        for (int i = 0; i < hitCount; i++)
            if (_overlapedColliders[i].TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);

        Exploded?.Invoke();

        _collider.enabled = false;
    }

    private void OnDrawGizmos()
        => Gizmos.DrawWireSphere(transform.position, _collider.bounds.extents.z);
}