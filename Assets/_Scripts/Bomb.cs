using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _timeToDetonate;
    [SerializeField] private int _damage;

    [SerializeField] private ParticleSystem _explodeVFX;

    [SerializeField] private SphereCollider _collider;

    private float _currentTime;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _timeToDetonate)
            {
                damageable.TakeDamage(_damage);

                _currentTime = 0;
            Instantiate(_explodeVFX, transform.position, transform.rotation,null);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _currentTime = 0;
    }

    private void OnDrawGizmos()
        => Gizmos.DrawWireSphere(transform.position, _collider.bounds.extents.z);
}
