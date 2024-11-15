using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private readonly int _deathKey = Animator.StringToHash("Die");
    private readonly int _takeDamageKey = Animator.StringToHash("TakeDamage");
    private readonly int _isIdlingKey = Animator.StringToHash("IsIdling");
    private readonly int _isRunning = Animator.StringToHash("IsRunning");

    private Animator _animator;

    public void Initialize()
        => _animator = GetComponent<Animator>();

    public void TriggerDeath() => _animator.SetTrigger(_deathKey);

    public void TriggerTakeDamage() => _animator.SetTrigger(_takeDamageKey);

    public void StartIdling() => _animator.SetBool(_isIdlingKey, true);

    public void StopIdling() => _animator.SetBool(_isIdlingKey, false);

    public void StartRunning() => _animator.SetBool(_isRunning, true);

    public void StopRunning() => _animator.SetBool(_isRunning, false);
}
