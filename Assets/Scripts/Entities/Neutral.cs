using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Neutral : NPC {
    
    // -- Variables --
    public float deleteBlurbTime = 2;

    // -- Components --
    private Image activeBlurb;
    
    public GameObject blurbParent;
    public Image blurbOuch;

    private void Start() {
        InvokeRepeating("CalculateMovement", 0, repeatMovementRate);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (activeBlurb != null) SetBlurbPosition();
        
        if (wasShot) {
            WasShot();
        }
        
        if (standStill) return;
        
        Move();
        Kill();
    }

    // -- Utility Methods --

    protected override void WasShot() {
        if (!printedBlood) PrintBlood();

        standStill = true;
        moveRight = false;

        CalculateTime();
    }
    
    private void PrintBlood() {
        base.PrintBlood();
        PrintOuchBlurb();
    }

    private void PrintOuchBlurb() {
        if (activeBlurb != null) return;
        
        activeBlurb = Instantiate(blurbOuch, blurbParent.transform.position, Quaternion.identity);
        activeBlurb.transform.SetParent(canvas.transform, false);
        
        SetBlurbPosition();

        foreach (Transform child in activeBlurb.transform) {
            // Delete children first
            Destroy(child.gameObject, deleteBlurbTime - 0.1f);
        }
        Destroy(activeBlurb, deleteBlurbTime);
    }

    private void SetBlurbPosition() {
        Vector2 viewportPoint = camera.WorldToViewportPoint(blurbParent.transform.position);
        activeBlurb.GetComponent<RectTransform>().anchorMin = viewportPoint;  
        activeBlurb.GetComponent<RectTransform>().anchorMax = viewportPoint;
    }
}