using System;

public interface IDamageable
{
    event Action TookDamage;

    event Action Died;

    void TakeDamage(int damage);
}