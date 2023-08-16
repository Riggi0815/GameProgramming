using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

//Script for the Gun Behaivior
public class Gun : MonoBehaviour {

    //Gun Data and Objects
    [SerializeField] private GunData _gunData;
    [SerializeField] private GameObject gun1;
    [SerializeField] private GameObject gun2;
    [SerializeField] private GameObject gun1UI;
    [SerializeField] private GameObject gun2UI;
    

    //bools for Gunhandling
    private bool isShooting, readyToShoot, reloading;

    //References to the Muzzleflash and the Camera
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzFlash;
    
    //Gets Called whenever the Player clicks the Left Mouse Butten or when he lets go of it
    public void OnShoot(InputAction.CallbackContext context)
    {
        //I use started and cancled because a weapon could be automatic
        //started calles when clicked and begin shooting
        if (context.started)
        {
            StartShot();
        }

        //canceled calles when relesed and end Shooting
        if (context.canceled)
        {
            EndShot();
        }
    }

    //Gets called when R key is pressed
    public void OnReloading(InputAction.CallbackContext context)
    {
        //called after R was pressed
        if (context.performed)
        {
            //Reload gets only called when mag is not full
            if (_gunData.ammoLeft < _gunData.magSize) {
                Reload();
            }
        }
    }

    //called when 1 is pressed and switches to Gun 1
    public void OnGunSwitch1(InputAction.CallbackContext context) {

        if (context.performed) {
            SwitchGunTo1();
        }
        
    }
    
    //called when 2 is pressed and switches to Gun 2
    public void OnGunSwitch2(InputAction.CallbackContext context) {

        if (context.performed) {
            SwitchGunTo2();
        }
        
    }

    //setting bool and fill up the magazine
    private void Awake() {
        
        readyToShoot = true;
        _gunData.ammoLeft = _gunData.magSize;
        //_gunData.bulltes = 999;
    }

    private void FixedUpdate()
    {
        if (isShooting && readyToShoot && !reloading && _gunData.ammoLeft > 0)
        {
            PerformShot();
        }
    }

    //set bool for shooting
    private void StartShot()
    {
        isShooting = true;
    }
    
    //set bool for stop shooting
    private void EndShot()
    {
        isShooting = false;
    }

    //Shots
    private void PerformShot()
    {
        readyToShoot = false;
        
        //ParticleEffect
        muzFlash.Play();

        //Target detection with Raycast
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, _gunData.range))
        {
            //Gets the Target Componetn
            Target target = hit.transform.GetComponent<Target>();
            
            //if somethings hit then Damage and Force Apply
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

        //After every Shot cooldown for firerate
        if (_gunData.ammoLeft >= 0)
        {
            Invoke("ResetShot", _gunData.fireRate);
            
            //no automatic Gun then stop
            if (!_gunData.isAutomatic)
            {
                EndShot();
            }
        }
    }
    

    //For FireRate
    private void ResetShot()
    {
        readyToShoot = true;
    }

    //Reload start
    private void Reload()
    {
        reloading = true;
        GetComponent<Animator>().Play(_gunData.reloadAnimation);
        Invoke("ReloadFinish", _gunData.reloadTime);
        
    }

    //reload end
    private void ReloadFinish()
    {
        GetComponent<Animator>().Play(_gunData.gunIdle);
        reloading = false;

        int reloadAmount = _gunData.magSize - _gunData.ammoLeft;
        if (_gunData.bulltes >= reloadAmount && _gunData.bulltes != 0) {
            _gunData.ammoLeft = _gunData.magSize;
            _gunData.bulltes = _gunData.bulltes - reloadAmount;
        }
        else {
            _gunData.ammoLeft = _gunData.ammoLeft + _gunData.bulltes;
            _gunData.bulltes = 0;
        }
    }

    //Gun Switching
    private void SwitchGunTo1() {
        gun1.SetActive(true);
        gun2.SetActive(false);
        gun1UI.SetActive(true);
        gun2UI.SetActive(false);
    }

    private void SwitchGunTo2() {
        gun1.SetActive(false);
        gun2.SetActive(true);
        gun1UI.SetActive(false);
        gun2UI.SetActive(true);
    }
}
