using UnityEngine;

public class SmallGattler : EnemyBase, IDamageable
{
    public new float Health { get; set; }
    public new float Shield { get; set; }
    
    [SerializeField] private new Transform leftWeaponSlot;
    [SerializeField] private new Transform rightWeaponSlot;
    [SerializeField] private new EnemyLeftWeaponType weaponTypeLeft = EnemyLeftWeaponType.Gattling;
    [SerializeField] private new EnemyRightWeaponType weaponTypeRight = EnemyRightWeaponType.Gattling;
    
}
