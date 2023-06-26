using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{

    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float range;
    public bool isAutomatic;

    [Header("Reloading")]
    public int ammoLeft;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    public float impactForce;
    public bool reloading;




}
