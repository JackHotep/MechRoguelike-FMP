using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class TestDummy : MonoBehaviour, IDamageable
{
    
    
    public float Health { get; set; }
    public float HealthCurrent { get; set; }
    
    public float Shield { get; set; }
    public float ShieldCurrent { get; set; }
    
    [SerializeField] private float healthStart = 10000f;
    [SerializeField] private float shieldStart = 10000f;
    
    public ElementType hullType = ElementType.Kinetic;
    public ElementType shieldType = ElementType.Kinetic;

    public float lastHitTaken;
    public float timeSinceDmg = 3f;
    
    private float shieldIncrease = 17f;
    private float rechargeRate = 1f;

    public GameObject damageTextKinetic;
    public GameObject damageTextElectric;
    public GameObject damageTextPlasma;

    
    private void Start()
    {
        Health = healthStart;
        Shield = shieldStart;
        HealthCurrent = Health;
        ShieldCurrent = Shield;
    }

    private void Update()
    {
        if ((Time.time > lastHitTaken + timeSinceDmg) && ShieldCurrent < Shield) { ShieldCurrent += shieldIncrease * rechargeRate; }
        else if (ShieldCurrent > Shield) { ShieldCurrent = Shield; }
    }

    public void Damage(float damage, ElementType damageType)
    {
        if (ShieldCurrent > 0)
        {
            var dmgMult = ElementCalc(shieldType, damageType);
            var finalDamage = damage * dmgMult;
            Debug.Log(finalDamage);
            ShieldCurrent -= finalDamage;
            DamageText(damageType, finalDamage);
            if (ShieldCurrent <= 0) { ShieldCurrent = 0; }
        }
        else if (ShieldCurrent <= 0)
        {
            var dmgMult = ElementCalc(hullType, damageType);
            var finalDamage = damage * dmgMult;
            Debug.Log(finalDamage);
            HealthCurrent -= finalDamage;
            DamageText(damageType, finalDamage);
            if (HealthCurrent <= 0)
            {
                HealthCurrent = 0;
                Death();
            }
        }
        lastHitTaken = Time.time;
    }

    public void Death()
    {
        Destroy(gameObject);
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

    private void DamageText(ElementType damageType, float finalDamage)
    {
        var roundedDamage = Mathf.Ceil(finalDamage);
        switch (damageType)
        {
            case ElementType.Kinetic:
                GameObject damageTextInstanceK = Instantiate(damageTextKinetic, transform);
                var dmgTextK = damageTextInstanceK.transform.GetChild(0).GetComponent<TMP_Text>();
                dmgTextK.SetText("<color=#FF8A00>" + roundedDamage.ToString());
                damageTextInstanceK.transform.LookAt(2* transform.position - Camera.main.transform.position);
                break;
            
            case ElementType.Electric:
                GameObject damageTextInstanceE = Instantiate(damageTextElectric, transform);
                var dmgTextE = damageTextInstanceE.transform.GetChild(0).GetComponent<TMP_Text>();
                dmgTextE.SetText("<color=#00E3FF>" + roundedDamage.ToString());
                damageTextInstanceE.transform.LookAt(2* transform.position - Camera.main.transform.position);
                break;
            
            case ElementType.Plasma:
                GameObject damageTextInstanceP = Instantiate(damageTextPlasma, transform);
                var dmgTextP = damageTextInstanceP.transform.GetChild(0).GetComponent<TMP_Text>();
                dmgTextP.SetText("<color=#FF0073>" + roundedDamage.ToString());
                damageTextInstanceP.transform.LookAt(2* transform.position - Camera.main.transform.position);
                break;
            default:
                Debug.Log("Damage processing error");
                break;
        }
    }
}


