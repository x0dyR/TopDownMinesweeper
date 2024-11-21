using System.Collections;
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

    private Character _character;

    private Animator _animator;

    private int _baseLayerIndex;
    private int _injuredLayerIndex;

    public void Initialize(Character character)
    {
        _character = character;

        _animator = GetComponent<Animator>();

        _character.Running += OnRunning;
        _character.Stopped += OnStopped;
        _character.Jumped += OnJumped;
        _character.TookDamage += OnTookDamage;
        _character.ReachedDafaultState += OnReachedDafaultState;
        _character.ReachedInjuredState += OnReachedLowHealth;
        _character.Died += OnDied;

        _baseLayerIndex = _animator.GetLayerIndex(BaseLayerName);
        _injuredLayerIndex = _animator.GetLayerIndex(InjuredLayerName);
    }

    private void OnReachedDafaultState() => ChangeLayerToBase();

    private void OnReachedLowHealth() => ChangeLayerToInjured();

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

    private void OnRunning(Vector3 position)
    {
        StopIdling();
        StartRunning();
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

    public void OnDied() => _animator.SetTrigger(_deathKey);

    public void OnTookDamage() => _animator.SetTrigger(_takeDamageKey);

    public void StartIdling() => _animator.SetBool(_isIdlingKey, true);

    public void StopIdling() => _animator.SetBool(_isIdlingKey, false);

    public void StartRunning() => _animator.SetBool(_isRunningKey, true);

    public void StopRunning() => _animator.SetBool(_isRunningKey, false);

    public void StartJumping() => _animator.SetBool(_isJumpingKey, true);

    public void StopJumping() => _animator.SetBool(_isJumpingKey, false);

    public void ChangeLayerToBase() => _animator.SetLayerWeight(_baseLayerIndex, 1.0f);

    public void ChangeLayerToInjured() => _animator.SetLayerWeight(_injuredLayerIndex, 1.0f);

    private IEnumerator JumpFor(float duration)
    {
        StartJumping();

        yield return new WaitForSeconds(duration);

        StopJumping();
    }
}
