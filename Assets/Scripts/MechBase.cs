using System;
using UnityEngine;

public abstract class MechBase : MonoBehaviour, IDamageable
{
    public ElementType hullType = ElementType.Kinetic;
    public ElementType shieldType = ElementType.Kinetic;
    
    public float Health { get; set; }
    public float HealthCurrent { get; set; }
    
    public float Shield { get; set; }
    public float ShieldCurrent { get; set; }

    private float lastHitTaken;
    
    private float rechargeRate = 1f;
    
    public void Damage(float damage, ElementType damageType)
    {
        throw new System.NotImplementedException();
    }

    public void Death()
    {
        throw new System.NotImplementedException();
    }


    private void Update()
    {
        throw new NotImplementedException();
    }
}
