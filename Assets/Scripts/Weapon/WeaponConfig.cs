using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Config", menuName = "Weapons/New Config", order = 1)]
public class WeaponConfig : ScriptableObject
{
    public LayerMask targetMask;
    public Vector3 spread = new Vector3(0.2f, 0.2f, 0.2f);
    public float fireRate = 0.2f;
    
}
