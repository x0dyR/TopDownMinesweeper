using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject _model;

    [SerializeField] private float _timeToDetonate;
    [SerializeField] private int _damage;

    [SerializeField] private ParticleSystem _explodeVFX;

    [SerializeField] private SphereCollider _collider;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _startExplodeSound;
    [SerializeField] private AudioClip _explodeSound;

    private Collider[] _overlapedColliders;

    [field: SerializeField] private Coroutine _explodeCoroutine;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _overlapedColliders = new Collider[32];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable _))
        {
            _explodeCoroutine ??= StartCoroutine(ExplodeTimer(_timeToDetonate));

            _audioSource.PlayOneShot(_startExplodeSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
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

        _audioSource.PlayOneShot(_explodeSound);

        _model.SetActive(false);
        _collider.enabled = false;

        Destroy(gameObject, _explodeSound.length);
        Instantiate(_explodeVFX, transform.position, transform.rotation, null);
    }

    private void OnDrawGizmos()
        => Gizmos.DrawWireSphere(transform.position, _collider.bounds.extents.z);
}