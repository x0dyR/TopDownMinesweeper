using System;

public class Health
{
    public event Action TookDamage;

    public event Action Died;

    private int _maxHealth;
    private int _currentHealth;

    public Health(int maxHealth, int currentHealth) //Помню, ты показывал пример с паттерном декоратор после применения максимальное хп становилось текущее                                                  
    {                                               
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
    }

    public int MaxHealth => _maxHealth; //Почему нельзя было трогать макс. хп из класса?

    public int CurrentHealth => _currentHealth;

    public bool IsAlive => _currentHealth > 0;

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException("not valid amount of damage");

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Died?.Invoke();
        }

        TookDamage?.Invoke();
    }
}
