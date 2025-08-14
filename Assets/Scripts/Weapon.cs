using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Assets/Weapons")]
public class Weapon : ScriptableObject
{
    public string ID;
    public string label;
    public string description;

    public int damageDealt;
    public int ammoCount;
    public int fireRate;

}
