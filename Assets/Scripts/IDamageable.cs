﻿using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
    void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitDirection);
    void Die();
}
