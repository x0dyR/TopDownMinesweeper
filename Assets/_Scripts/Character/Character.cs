using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour, IDamageable
{
    public event Action TookDamage;

    private const float InjuredStateThreshold = .3f;

    [SerializeField] private NavMeshAgent _navAgent;

    [SerializeField] private float _speed;

    [SerializeField] private CharacterView _view;

    [SerializeField] private GameObject _pathGoalObject;

    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private int _maxHealth;

    [SerializeField] private int _currentHealth;

    private InputSystem _input;

    private Raycaster _raycaster;

    private IMover _mover;

    private Vector3 _inputDirection;

    private PathGoalPoint _pathGoalVisualizer;

    private Health _health;

    private bool _isAlive;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        _input = new InputSystem();

        _raycaster = new Raycaster();

        _mover = new NavMeshMover(_navAgent);

        _pathGoalVisualizer = new PathGoalPoint(_pathGoalObject);

        _health = new Health(_maxHealth, _maxHealth);

        _health.TookDamage += OnTookDamage;
        _health.Died += OnDying;

        _isAlive = _health.CurrentHealth > 0 ? true : false;

        _currentHealth = _health.CurrentHealth;

        _view.Initialize();
    }

    private void Update()
    {
        if (_isAlive)
        {
            if (_input.RightMousePressed())
            {
                _inputDirection = _raycaster.RaycastToGround(_input.ReadMousePosition(), _groundLayer);
                _pathGoalVisualizer.MoveTo(_inputDirection);
            }

            _mover.ProcessMove(_inputDirection);

            if (_navAgent.velocity.magnitude < _navAgent.stoppingDistance)
            {
                _view.StopRunning();
                _view.StartIdling();
            }
            else
            {
                _view.StartRunning();
                _view.StopIdling();
            }
        }

        if (_isAlive == false)
        {
            _inputDirection = transform.position;
            _pathGoalVisualizer.MoveTo(_inputDirection);
            _mover.ProcessMove(_inputDirection);
        }
    }

    private void OnDisable()
    {
        _health.TookDamage -= OnDying;
        _health.TookDamage -= OnTookDamage;
    }

    private void OnDying()
    {
        _view.TriggerDeath();
        _isAlive = false;
        Debug.Log("Im dead ;(");
    }

    private void OnTookDamage()
    {
        _view.TriggerTakeDamage();
        TookDamage?.Invoke();

        _currentHealth = _health.CurrentHealth;

        if ((_health.CurrentHealth / (float)_health.MaxHealth) < InjuredStateThreshold)
            _view.ChangeLayerToInjured();
        else
            _view.ChangeLayerToBase();
    }

    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
    }
}
