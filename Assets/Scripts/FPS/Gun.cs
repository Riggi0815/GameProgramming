using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{

    [SerializeField] private float _damage;
    [SerializeField] private float _range;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _impactForce;
    [SerializeField] private bool _isAutomatic;
    [SerializeField] private int _magSize;
    [SerializeField] private float _reloadTime;
    [SerializeField] private int ammoLeft;

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

    private void Awake()
    {
        readyToShoot = true;
        ammoLeft = _magSize;
    }

    private void Update()
    {
        if (isShooting && readyToShoot && !reloading && ammoLeft > 0)
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
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, _range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(_damage);
            }

            if (hit.rigidbody)
            {
                hit.rigidbody.AddForce(-hit.normal * _impactForce); 
            }
        }

        ammoLeft--;

        if (ammoLeft >= 0)
        {
            Invoke("ResetShot", _fireRate);

            if (!_isAutomatic)
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
        Invoke("ReloadFinish", _reloadTime);
    }

    private void ReloadFinish()
    {
        ammoLeft = _magSize;
        reloading = false;
    }

}
