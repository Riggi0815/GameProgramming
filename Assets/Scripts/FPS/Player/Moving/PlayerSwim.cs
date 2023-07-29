using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwim : MonoBehaviour {

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float depthBeforeSubmerged;
    [SerializeField] private float displacementAmount;

    private void FixedUpdate() {

        if (transform.position.y <= -0.8f) {
            float displacementMultiplie = Mathf.Clamp01(-transform.position.y / depthBeforeSubmerged) * displacementAmount;
            playerMovement._velocity.y = Mathf.Abs(playerMovement._gravity) / displacementMultiplie;

        }
        
    }
}
