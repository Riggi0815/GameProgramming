using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulltes : MonoBehaviour {

    [SerializeField] private GameObject curBulltes;
    [SerializeField] private GameObject maxBulltes;
    [SerializeField] private GunData _gunData;

    private string curBulltesText;
    private string maxBulltesText;
    
    private void Awake() {
        curBulltes.GetComponent<Text>().text = _gunData.ammoLeft.ToString();
        maxBulltes.GetComponent<Text>().text = _gunData.bulltes.ToString();
    }

    private void FixedUpdate() {
        curBulltes.GetComponent<Text>().text = _gunData.ammoLeft.ToString();
        maxBulltes.GetComponent<Text>().text = _gunData.bulltes.ToString();
    }
}

    
