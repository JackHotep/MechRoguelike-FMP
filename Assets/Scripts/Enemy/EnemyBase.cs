using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public float Health { get; set; }
    public float HealthCurrent { get; set; }
    
    public float Shield { get; set; }
    public float ShieldCurrent { get; set; }
    
    protected Transform leftWeaponSlot;
    protected Transform rightWeaponSlot;
    protected LeftWeaponType weaponTypeLeft;
    protected RightWeaponType weaponTypeRight;
    protected List<Weapon> listWeaponsLeft;
    protected List<Weapon> listWeaponsRight;
    
    public Weapon activeLeft;
    public Weapon activeRight;
    
    protected float stoppingDistance = 15f;
    protected float retreatDistance = 5f;

    protected float turnSpeed = 2f;

    private Transform player;
    private NavMeshAgent navAgent;

    private float moveTimer;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = this.gameObject.GetComponent<NavMeshAgent>();
        Initialize();
    }

    protected virtual void Initialize()
    {
        HealthCurrent = Health;
        ShieldCurrent = Shield;
        Weapon slotL = listWeaponsLeft.Find(slotL => slotL.weaponTypeL == weaponTypeLeft);
        Weapon slotR = listWeaponsRight.Find(slotR => slotR.weaponTypeR == weaponTypeRight);
        activeLeft = slotL;
        activeRight = slotR;
        slotL.Spawn(leftWeaponSlot, this);
        slotR.Spawn(rightWeaponSlot, this);
    }

    protected virtual void Update()
    {
        Vector3 targetDirection = player.position - transform.position;
        var rotation = Quaternion.LookRotation(targetDirection);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
        if (Vector3.Distance(transform.position, player.position) < stoppingDistance && Vector3.Distance(transform.position, player.position) > retreatDistance)
        {
            navAgent.SetDestination(transform.position);
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            Ray ray = new Ray(transform.position, targetDirection);
            RaycastHit hitInfo = new RaycastHit();
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject == player.gameObject)
                {
                    Attack();
                }
            }
        }
        else if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            navAgent.SetDestination(player.position);
        }
        else if (Vector3.Distance(transform.position, player.position) < retreatDistance)
        {
            Vector3 dirFromPlayer = transform.position - player.position;
            Vector3 newPos = transform.position + dirFromPlayer;
            navAgent.SetDestination(newPos);
        }
        
    }

    protected virtual void Move()
    {
        navAgent.SetDestination(transform.position + Random.insideUnitSphere * 5);
        moveTimer = 0;
    }

    protected virtual void Attack()
    {
        moveTimer += Time.deltaTime;
        activeLeft.Shoot();
        activeRight.Shoot();
        
        if (moveTimer > Random.Range(5,10))
        {
            Move();
        }
    }
    

    public void Damage(float damage, ElementType damageType)
    {
        throw new System.NotImplementedException();
    }

    public void Death()
    {
        throw new System.NotImplementedException();
    }

}
