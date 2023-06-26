using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] private GunData gunData;

    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem musFlash;
    
    private float timeSinceLastShot;
    private bool readyToShot;
        
    private void Start()
    {
        PlayerShootingFPS.shootInput += Shoot;
        PlayerShootingFPS.reloadInput += Reload;
    }
    
    private void Awake()
    {
        readyToShot = true;
        gunData.ammoLeft = gunData.magSize;
    }

    private bool CanShoot() => !gunData.reloading && readyToShot && gunData.ammoLeft > 0 && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if (CanShoot())
            {
                musFlash.Play();

                RaycastHit hit;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, gunData.range))
                {
                    IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);

                    if (hit.rigidbody)
                    {
                        hit.rigidbody.AddForce(-hit.normal * gunData.impactForce);
                    }
                }

                gunData.ammoLeft--;
                timeSinceLastShot = 0;
                OnGunShot();

                if (gunData.ammoLeft >= 0)
                {
                    Invoke("ResetShot", gunData.fireRate);
                    
                }
            }
    }

    public void Reload()
    {
        if (!gunData.reloading)
        {
            gunData.reloading = true;
            Invoke("ReloadFinish", gunData.reloadTime);
        }
    }
    
    private void ReloadFinish()
    {
        gunData.ammoLeft = gunData.magSize;
        gunData.reloading = false;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShot()
    {
        
    }
}
