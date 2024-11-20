using System;

public class Health : IDamageable
{
    public event Action TookDamage;

    public event Action Died;

    private int _maxHealth;
    private int _currentHealth;

    public Health(int maxHealth, int currentHealth) //When a new Health instance is created current health value will be set as the initial current health.
    {
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
    }

    public int MaxHealth => _maxHealth;

    public int CurrentHealth => _currentHealth;

    public bool IsAlive => _currentHealth > 0;

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException("not valid amount of damage");

        _currentHealth -= damage;

        TookDamage?.Invoke();

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Died?.Invoke();
        }
    }
}
