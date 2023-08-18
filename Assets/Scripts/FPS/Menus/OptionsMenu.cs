using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public GameObject textX;
    public GameObject textY;

    public Slider sliderX;
    public Slider sliderY;

    private void Start() {
        sliderX.value = MouseLook.mouseSensitivityX;
        sliderY.value = MouseLook.mouseSensitivityY;
        textX.GetComponent<TMP_Text>().text = MouseLook.mouseSensitivityX.ToString("F2");
        textY.GetComponent<TMP_Text>().text = MouseLook.mouseSensitivityY.ToString("F2");
    }

    //Fullscreen Toggle
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    //Set Sensitivity for x Axis
    public void SetSensitivityX(float sensitivityX) {
        MouseLook.mouseSensitivityX = sensitivityX;
        textX.GetComponent<TMP_Text>().text = sensitivityX.ToString("F2");

    }
    
    //Set Sensitivity for y Axis
    public void SetSensitivityY(float sensitivityY) {
        MouseLook.mouseSensitivityY = sensitivityY;
        textY.GetComponent<TMP_Text>().text = sensitivityY.ToString("F2");
    }
    
}
