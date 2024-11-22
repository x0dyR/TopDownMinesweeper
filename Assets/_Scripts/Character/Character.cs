using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour, IDamageable
{
    public event Action TookDamage;
    public event Action ReachedDafaultState;
    public event Action ReachedInjuredState;
    public event Action Died;

    public event Action<Vector3> Running;
    public event Action Stopped;
    public event Action<float> Jumped;

    private const float InjuredStateHealthThreshold = .3f;

    [SerializeField] private NavMeshAgent _navAgent;

    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    [SerializeField] private AnimationCurve _jumpCurve;

    private Health _health;

    private InputSystem _input;
    private Raycaster _raycaster;

    private IMover _mover;
    private Vector3 _inputDirection;

    private Coroutine _jumpCoroutine;

    public NavMeshAgent NavAgent => _navAgent;

    public void Initialize(InputSystem input, Raycaster raycaster, IMover mover, Health health)
    {
        _input = input;
        _raycaster = raycaster;
        _mover = mover;
        _health = health;

        _health.TookDamage += OnTookDamage;
        _health.Died += OnDied;

        _currentHealth = _health.CurrentHealth;
    }

    private void Update()
    {
        if (_health.IsAlive)
        {
            if (_input.RightMousePressed())
                _inputDirection = _raycaster.RaycastToGround(_input.ReadMousePosition(), _groundLayer);

            if (_navAgent.isOnOffMeshLink)
            {
                _jumpCoroutine ??= StartCoroutine(Jump());

                return;
            }

            _mover.ProcessMove(_inputDirection);

            if (_navAgent.velocity.magnitude < _navAgent.stoppingDistance)
                Stopped?.Invoke();
            else
                Running?.Invoke(_inputDirection);
        }

        if (_health.IsAlive == false)
        {
            _inputDirection = transform.position;
            _mover.ProcessMove(_inputDirection);
        }
    }

    private void OnDisable()
    {
        _health.TookDamage -= OnTookDamage;
        _health.Died -= OnDied;
    }

    public void TakeDamage(int damage) => _health.TakeDamage(damage);

    private void OnTookDamage()
    {
        _currentHealth = _health.CurrentHealth;
        TookDamage?.Invoke();

        if ((_health.CurrentHealth / (float)_health.MaxHealth) < InjuredStateHealthThreshold)
            ReachedInjuredState?.Invoke();
        else
            ReachedDafaultState?.Invoke();
    }

    private void OnDied() => Died?.Invoke();

    private IEnumerator Jump()
    {
        _navAgent.isStopped = true;

        OffMeshLinkData data = _navAgent.currentOffMeshLinkData;

        Vector3 startPos = _navAgent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * _navAgent.baseOffset;

        float duration = (startPos - endPos).magnitude / _navAgent.speed;

        Jumped?.Invoke(duration);

        float progress = 0;

        while (progress < duration)
        {
            float yOffset = _jumpCurve.Evaluate(progress / duration);
            _navAgent.transform.position = Vector3.Lerp(startPos, endPos, progress / duration) + yOffset * Vector3.up;

            transform.rotation = Quaternion.LookRotation(endPos - startPos);

            progress += Time.deltaTime;
            yield return null;
        }

        _navAgent.CompleteOffMeshLink();
        _navAgent.isStopped = false;
        _jumpCoroutine = null;
    }
}