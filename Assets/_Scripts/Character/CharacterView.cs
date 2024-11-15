using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private const int BaseLayer = 0;
    private const int InjuredLayer = 1;

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

    public void ChangeLayerToBase() => _animator.SetLayerWeight(BaseLayer, 1.0f);

    public void ChangeLayerToInjured() => _animator.SetLayerWeight(InjuredLayer, 1.0f);
}
