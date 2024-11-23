using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private readonly int _deathKey = Animator.StringToHash("Die");
    private readonly int _takeDamageKey = Animator.StringToHash("TakeDamage");
    private readonly int _isIdlingKey = Animator.StringToHash("IsIdling");
    private readonly int _isRunningKey = Animator.StringToHash("IsRunning");
    private readonly int _isJumpingKey = Animator.StringToHash("IsJumping");

    private const string BaseLayerName = "Base Layer";
    private const string InjuredLayerName = "Injured Layer";

    private const string DisapearProgressProperty = "_DisapearProgress";
    private const string TookDamagePropetry = "_TookDamage";

    [SerializeField] private float _dissolveSpeed;
    [SerializeField] private float _takeDamageDuration;

    private Character _character;

    private Animator _animator;

    private int _baseLayerIndex;
    private int _injuredLayerIndex;

    private List<Material> _materials;

    public void Initialize(Character character)
    {
        _character = character;

        _animator = GetComponent<Animator>();

        List<Renderer> renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        _materials = new();

        foreach (var renderer in renderers)
            _materials.Add(renderer.material);

        _baseLayerIndex = _animator.GetLayerIndex(BaseLayerName);
        _injuredLayerIndex = _animator.GetLayerIndex(InjuredLayerName);

        _character.Running += OnRunning;
        _character.Stopped += OnStopped;
        _character.Jumped += OnJumped;
        _character.TookDamage += OnTookDamage;
        _character.ReachedDafaultState += OnReachedDafaultState;
        _character.ReachedInjuredState += OnReachedLowHealth;
        _character.Died += OnDied;
    }

    private void OnDisable()
    {
        _character.Running -= OnRunning;
        _character.Stopped -= OnStopped;
        _character.Jumped -= OnJumped;
        _character.TookDamage -= OnTookDamage;
        _character.ReachedDafaultState -= OnReachedDafaultState;
        _character.ReachedInjuredState -= OnReachedLowHealth;
        _character.Died -= OnDied;
    }

    private void OnRunning(Vector3 position)
    {
        StopIdling();
        StartRunning();
    }

    private void OnStopped()
    {
        StopRunning();
        StartIdling();
    }

    private void OnJumped(float duration)
    {
        StopIdling();
        StopRunning();

        StartCoroutine(JumpFor(duration));
    }

    public void StartDisapear()
    {
        StartCoroutine(Disapear(_materials));
    }
    public void StartTakeDamage()
    {
        StartCoroutine(TakeDamage(_materials));
    }

    private void OnTookDamage() => _animator.SetTrigger(_takeDamageKey);

    private void OnReachedDafaultState() => ChangeLayerToBase();

    private void OnReachedLowHealth() => ChangeLayerToInjured();

    private void OnDied() => _animator.SetTrigger(_deathKey);

    private void StartIdling() => _animator.SetBool(_isIdlingKey, true);

    private void StopIdling() => _animator.SetBool(_isIdlingKey, false);

    private void StartRunning() => _animator.SetBool(_isRunningKey, true);

    private void StopRunning() => _animator.SetBool(_isRunningKey, false);

    private void StartJumping() => _animator.SetBool(_isJumpingKey, true);

    private void StopJumping() => _animator.SetBool(_isJumpingKey, false);

    private void ChangeLayerToBase() => _animator.SetLayerWeight(_baseLayerIndex, 1.0f);

    private void ChangeLayerToInjured() => _animator.SetLayerWeight(_injuredLayerIndex, 1.0f);

    private IEnumerator JumpFor(float duration)
    {
        StartJumping();

        yield return new WaitForSeconds(duration);

        StopJumping();
    }

    private IEnumerator Disapear(IEnumerable<Material> materials)
    {
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * _dissolveSpeed;

            foreach (var material in materials)
                material.SetFloat(DisapearProgressProperty, progress);

            yield return null;
        }
        Destroy(_character.gameObject);
    }

    private IEnumerator TakeDamage(IEnumerable<Material> materials)
    {
        foreach (var material in materials)
            material.SetFloat(TookDamagePropetry, 1);

        yield return new WaitForSeconds(_takeDamageDuration);

        foreach (var material in materials)
            material.SetFloat(TookDamagePropetry, 0);
    }
}
