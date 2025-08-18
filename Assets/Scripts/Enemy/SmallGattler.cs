using UnityEngine;

public class SmallGattler : EnemyBase, IDamageable
{
    public new float Health { get; set; }
    public new float Shield { get; set; }
    
    [SerializeField] private new Transform leftWeaponSlot;
    [SerializeField] private new Transform rightWeaponSlot;
    [SerializeField] private new LeftWeaponType weaponTypeLeft = LeftWeaponType.Gattling;
    [SerializeField] private new RightWeaponType weaponTypeRight = RightWeaponType.Gattling;
    
}
