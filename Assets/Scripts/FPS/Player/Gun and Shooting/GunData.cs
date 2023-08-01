using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GunData : ScriptableObject {

    
    public float damage;
    public float range;
    public float fireRate;
    public float impactForce;
    public bool isAutomatic;
    public int magSize;
    public float reloadTime;
    public int ammoLeft;
    public string reloadAnimation;
    public string gunIdle;

}
