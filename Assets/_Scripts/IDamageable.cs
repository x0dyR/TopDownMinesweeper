using System;

public interface IDamageable
{
    event Action TookDamage;

    void TakeDamage(int damage);
}