using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    public GunData rifleGunData;
    public GunData pistolGunData;
    
    private void OnTriggerEnter(Collider other) {

        rifleGunData.bulltes = 999;
        rifleGunData.ammoLeft = rifleGunData.magSize;
        pistolGunData.bulltes = 999;
        pistolGunData.ammoLeft = pistolGunData.magSize;
        Destroy(gameObject);
        
    }
}
