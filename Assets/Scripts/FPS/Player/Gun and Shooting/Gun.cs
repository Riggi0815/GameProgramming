using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour {

    [SerializeField] private GunData _gunData;
    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject gun2;

    private bool isShooting, readyToShoot, reloading;

    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzFlash;
    
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartShot();
        }

        if (context.canceled)
        {
            EndShot();
        }
    }

    public void OnReloading(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Reload();
        }
    }

    public void OnGunSwitch1(InputAction.CallbackContext context) {

        if (context.performed) {
            SwitchGunTo1();
        }
        
    }
    
    public void OnGunSwitch2(InputAction.CallbackContext context) {

        if (context.performed) {
            SwitchGunTo2();
        }
        
    }

    private void Awake()
    {
        readyToShoot = true;
        _gunData.ammoLeft = _gunData.magSize;
    }

    private void FixedUpdate()
    {
        if (isShooting && readyToShoot && !reloading && _gunData.ammoLeft > 0)
        {
            PerformShot();
        }
    }

    private void StartShot()
    {
        isShooting = true;
    }
    
    private void EndShot()
    {
        isShooting = false;
    }

    private void PerformShot()
    {
        readyToShoot = false;
        
        muzFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, _gunData.range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.PlayerDamage(_gunData.damage);
            }

            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(-hit.normal * _gunData.impactForce); 
            }
        }

        _gunData.ammoLeft--;

        if (_gunData.ammoLeft >= 0)
        {
            Invoke("ResetShot", _gunData.fireRate);

            if (!_gunData.isAutomatic)
            {
                EndShot();
            }
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        GetComponent<Animator>().Play(_gunData.reloadAnimation);
        Invoke("ReloadFinish", _gunData.reloadTime);
    }

    private void ReloadFinish()
    {
        GetComponent<Animator>().Play(_gunData.gunIdle);
        _gunData.ammoLeft = _gunData.magSize;
        reloading = false;
    }

    private void SwitchGunTo1() {
        gun1.SetActive(true);
        gun2.SetActive(false);
    }
    
    private void SwitchGunTo2() {
        gun1.SetActive(false);
        gun2.SetActive(true);
    }

}
