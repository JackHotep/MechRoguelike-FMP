using UnityEngine;

public interface IDamageable
{
    void Damage(float damage, ElementType damageType);
    
    void Death();
    
    float Health { get; set; }
    
    float HealthCurrent { get; set; }
}
