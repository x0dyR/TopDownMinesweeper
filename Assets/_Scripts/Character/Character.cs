using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour, IDamageable
{
    private const float InjuredStateHealthThreshold = .3f;

    [SerializeField] private NavMeshAgent _navAgent;

    [SerializeField] private CharacterView _view;

    [SerializeField] private PathGoalObject _pathGoalObject;

    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;

    private Health _health;

    private InputSystem _input;
    private Raycaster _raycaster;

    private IMover _mover;
    private Vector3 _inputDirection;

    private PathGoalPoint _pathGoalVisualizer;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        _input = new InputSystem();
        _raycaster = new Raycaster();

        _mover = new NavMeshMover(_navAgent);

        _pathGoalVisualizer = new PathGoalPoint(_pathGoalObject);

        _health = new Health(_maxHealth, _maxHealth);

        _health.TookDamage += OnTookDamage;
        _health.Died += OnDied;

        _currentHealth = _health.CurrentHealth;

        _view.Initialize();
    }

    private void Update()
    {
        if (_health.IsAlive)
        {
            if (_input.RightMousePressed())
            {
                _inputDirection = _raycaster.RaycastToGround(_input.ReadMousePosition(), _groundLayer);
                _pathGoalVisualizer.MoveTo(_mover.GoalPosition);
            }

            _mover.ProcessMove(_inputDirection);

            if (_navAgent.velocity.sqrMagnitude < _navAgent.stoppingDistance * _navAgent.stoppingDistance)
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

        if (_health.IsAlive == false)
        {
            _inputDirection = transform.position;
            _pathGoalVisualizer.MoveTo(_inputDirection);
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

        _view.TriggerTakeDamage();

        if ((_health.CurrentHealth / (float)_health.MaxHealth) < InjuredStateHealthThreshold)
            _view.EnableInjuredLayer();
        else
            _view.DisableInjuredLayer();
    }

    private void OnDied() => _view.TriggerDeath();
}