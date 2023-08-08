using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulltes : MonoBehaviour {

    public static int curBulltesInt;
    public static int maxBulltesInt;

    public GameObject curBulltes;
    public GameObject maxBulltes;

    private Text curBulltesText;
    private Text maxBulltesText;

    private void Start() {
        curBulltesText = curBulltes.GetComponent<Text>();
        maxBulltesText = maxBulltes.GetComponent<Text>();
        curBulltesText.text = curBulltesInt.ToString();
        maxBulltesText.text = maxBulltesInt.ToString();
    }

    private void Update() {
        curBulltesText.text = curBulltesInt.ToString();
        maxBulltesText.text = maxBulltesInt.ToString();
    }
}

    
