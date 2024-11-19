using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private readonly int _deathKey = Animator.StringToHash("Die");
    private readonly int _takeDamageKey = Animator.StringToHash("TakeDamage");
    private readonly int _isIdlingKey = Animator.StringToHash("IsIdling");
    private readonly int _isRunning = Animator.StringToHash("IsRunning");

    private const string BaseLayerName = "Base Layer";
    private const string InjuredLayerName = "Injured Layer";

    private Animator _animator;

    private int _baseLayerIndex;
    private int _injuredLayerIndex;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();

        _baseLayerIndex = _animator.GetLayerIndex(BaseLayerName);
        _injuredLayerIndex = _animator.GetLayerIndex(InjuredLayerName);
    }

    public void TriggerDeath() => _animator.SetTrigger(_deathKey);

    public void TriggerTakeDamage() => _animator.SetTrigger(_takeDamageKey);

    public void StartIdling() => _animator.SetBool(_isIdlingKey, true);

    public void StopIdling() => _animator.SetBool(_isIdlingKey, false);

    public void StartRunning() => _animator.SetBool(_isRunning, true);

    public void StopRunning() => _animator.SetBool(_isRunning, false);

    public void ChangeLayerToBase() => _animator.SetLayerWeight(_baseLayerIndex, 1.0f);

    public void ChangeLayerToInjured() => _animator.SetLayerWeight(_injuredLayerIndex, 1.0f);
}
