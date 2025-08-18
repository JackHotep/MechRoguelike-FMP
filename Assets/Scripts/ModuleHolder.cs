using System.Collections.Generic;
using UnityEngine;

public class ModuleHolder : MonoBehaviour
{
    [SerializeField] private LeftWeaponType leftWeapon;
    [SerializeField] private RightWeaponType rightWeapon;
    [SerializeField] private Transform leftParent;
    [SerializeField] private Transform rightParent;
    [SerializeField] private List<Weapon> weapons;

    public Weapon activeLeft;
    public Weapon activeRight;
}
