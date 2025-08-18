using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public ElementType hullType = ElementType.Kinetic;
    public ElementType shieldType = ElementType.Kinetic;

    public Weapon leftWeapon;
    public Weapon rightWeapon;
    
    public float Health { get; set; }
    public float HealthCurrent { get; set; }
    
    public float Shield { get; set; }
    public float ShieldCurrent { get; set; }
    
    private float lastHitTaken;
    private float timeSinceDmg = 3f;

    private float shieldIncrease = 17f;
    private float rechargeRate = 1f;
    
    private PlayerInputManager inputMan;
    
    public void Damage(float damage, ElementType damageType)
    {
        if (ShieldCurrent > 0)
        {
            var dmgMult = ElementCalc(shieldType, damageType);
            ShieldCurrent -= damage * dmgMult;
            if (ShieldCurrent <= 0) { ShieldCurrent = 0; }
        }
        else if (ShieldCurrent <= 0)
        {
            var dmgMult = ElementCalc(hullType, damageType);
            HealthCurrent -= damage * dmgMult;
            if (HealthCurrent <= 0)
            {
                HealthCurrent = 0;
                Death();
            }
        }
        lastHitTaken = Time.time;
    }

    private float ElementCalc(ElementType mechType, ElementType dmgType)
    {
        switch (mechType)
        {
            case ElementType.Kinetic:
                switch (dmgType)
                {
                    case ElementType.Kinetic:
                        return 1f;
                    case ElementType.Electric:
                        return 0.5f;
                    case ElementType.Plasma:
                        return 1.5f;
                    default:
                        Debug.Log("No damage type found, returning default value...");
                        return 1f;
                }
            case ElementType.Electric:
                switch (dmgType)
                {
                    case ElementType.Kinetic:
                        return 1.5f;
                    case ElementType.Electric:
                        return 1f;
                    case ElementType.Plasma:
                        return 0.5f;
                    default:
                        Debug.Log("No damage type found, returning default value...");
                        return 1f;
                }
            case ElementType.Plasma:
                switch (dmgType)
                {
                    case ElementType.Kinetic:
                        return 0.5f;
                    case ElementType.Electric:
                        return 1.5f;
                    case ElementType.Plasma:
                        return 1f;
                    default:
                        Debug.Log("No damage type found, returning default value...");
                        return 1f;
                }
            default:
                Debug.Log("No armour type found, returning default value...");
                return 1f;
        }
    }

    public void Death()
    {
        throw new System.NotImplementedException();
        // For now, freeze time with menu
    }

    private void Awake()
    {
        HealthCurrent = Health;
        ShieldCurrent = Shield;
    }

    private void Start()
    {
        inputMan = PlayerInputManager.Inst;
    }

    private void Update()
    {
        if ((Time.time > lastHitTaken + timeSinceDmg) && ShieldCurrent < 200) { ShieldCurrent += shieldIncrease * rechargeRate; }
        else if (ShieldCurrent > 200) { ShieldCurrent = 200f; }

        //if (inputMan.LeftShoot)
    }
}
