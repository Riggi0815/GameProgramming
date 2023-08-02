using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Slider slider;

    //when Player spawns he gets his Health assigned
    public void SetMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health;
    }

    //Updating the New Health
    public void SetHealth(int health) {
        slider.value = health;
    }

}
