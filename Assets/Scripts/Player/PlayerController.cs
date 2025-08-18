using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movement Values")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float boostMult = 2f;
    
    [Header("Jump Values")]
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private float grav = 9.81f;

    [Header("View Values")] 
    [SerializeField] private float aimSens = 2f;
    [SerializeField] private float vertClamp = 70f;
    
    private float lastHitTaken;
    private float timeSinceDmg = 3f;

    private float shieldIncrease = 17f;
    private float rechargeRate = 1f;
    
    public float Health { get; set; }
    public float HealthCurrent { get; set; }
    
    public float Shield { get; set; }
    public float ShieldCurrent { get; set; }
    
    [SerializeField] private float healthStart = 100f;
    [SerializeField] private float shieldStart = 200f;
    
    private CharacterController charCon;
    private Camera camera;
    private PlayerInputManager inputMan;
    private Vector3 movementCurrent = Vector3.zero;
    private float vertRot;

    [SerializeField] private Transform leftWeaponSlot;
    [SerializeField] private Transform rightWeaponSlot;
    [SerializeField] private LeftWeaponType weaponTypeLeft = LeftWeaponType.Gattling;
    [SerializeField] private RightWeaponType weaponTypeRight = RightWeaponType.Gattling;
    [SerializeField] private List<Weapon> listWeaponsLeft;
    [SerializeField] private List<Weapon> listWeaponsRight;

    public Weapon activeLeft;
    public Weapon activeRight;

    //public int levelCount = 0;   DO THIS IN PLAYERPROGRESS SCRIPT
    
    
    public ElementType hullType = ElementType.Kinetic; //Default - Kinetic
    public ElementType shieldType = ElementType.Kinetic; //Default - Kinetic
    
    private void Awake()
    {
        charCon = GetComponent<CharacterController>();
        camera = Camera.main;
        Health = healthStart;
        Shield = shieldStart;
        HealthCurrent = Health;
        ShieldCurrent = Shield;
    }

    private void Start()
    {
        //var startRoutine = StartRoutine();
        
        // Fixes NullReferenceException
        inputMan = PlayerInputManager.Inst;

        Weapon slotL = listWeaponsLeft.Find(slotL => slotL.weaponTypeL == weaponTypeLeft);
        Weapon slotR = listWeaponsRight.Find(slotR => slotR.weaponTypeR == weaponTypeRight);

        activeLeft = slotL;
        activeRight = slotR;
        
        Debug.Log(slotL);
        Debug.Log(slotR);

        slotL.Spawn(leftWeaponSlot, this);
        slotR.Spawn(rightWeaponSlot, this);
    }
/*
    private IEnumerator StartRoutine()
    {
        inputMan = PlayerInputManager.Inst;

        yield return new WaitForSeconds(0.05f);
        
        Weapon slotL = weapons.Find(slotL => slotL.weaponType == weaponTypeLeft);
        Weapon slotR = weapons.Find(slotR => slotR.weaponType == weaponTypeRight);

        activeLeft = slotL;
        activeRight = slotR;

        slotL.Spawn(leftWeaponSlot, this);
        slotR.Spawn(rightWeaponSlot, this);
        yield return true;
    }
    */
    
    private void Update()
    {
        if ((Time.time > lastHitTaken + timeSinceDmg) && ShieldCurrent < Shield) { ShieldCurrent += shieldIncrease * rechargeRate; }
        else if (ShieldCurrent > Shield) { ShieldCurrent = Shield; }
        MovementManager();
        ViewManager();
        FireWeapon();
    }

    private void MovementManager()
    {
        float speed = moveSpeed * (inputMan.Boost > 0 ? boostMult : 1f);
        
        // x for x, 0 for y and y for z because vector2, and we dont need to use y for this
        Vector3 inputDir = new Vector3(inputMan.Move.x, 0f, inputMan.Move.y);
        Vector3 worldDir = transform.TransformDirection(inputDir);
        worldDir.Normalize();

        movementCurrent.x = worldDir.x * speed;
        movementCurrent.z = worldDir.z * speed;

        JumpManager();
        charCon.Move(movementCurrent * Time.deltaTime);
    }

    private void JumpManager()
    {
        if (charCon.isGrounded)
        {
            movementCurrent.y = -0.5f;

            if (inputMan.Jump) { movementCurrent.y = jumpPower; }
            
        }
        else { movementCurrent.y -= grav * Time.deltaTime; }
    }

    private void ViewManager()
    {
        float xRot = inputMan.Look.x * aimSens;
        transform.Rotate(0, xRot, 0);

        vertRot -= inputMan.Look.y * aimSens;
        vertRot = Mathf.Clamp(vertRot, -vertClamp, vertClamp);
        camera.transform.localRotation = Quaternion.Euler(vertRot, 0, 0);
    }

    private void FireWeapon()
    {
        switch (inputMan.GetAttack())
        {
            case 1: activeLeft.Shoot(); Debug.Log("Left shoot with " + activeLeft);
                break;
            case 2: activeRight.Shoot(); Debug.Log("Right shoot with " + activeRight);
                break;
            case 3: activeLeft.Shoot(); activeRight.Shoot(); Debug.Log("Both shoot with " + activeLeft + activeRight);
                break;
            default: break;
        }
    }


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
        throw new NotImplementedException();
    }
}
