using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Neutral : NPC {
    
    // -- Variables --
    public float deleteBlurbTime = 2;
    public float deleteBlurbTextOffset = 0.05f;

    // -- Components --
    private Image activeBlurb;
    
    public GameObject blurbParent;
    public Image blurbOuch;

    protected override void Start() {
        InvokeRepeating("CalculateMovement", 0, repeatMovementRate);
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate() {
        Kill();
        SetBlurbPosition();

        if (wasShot) WasShot();
        if (standStill) return;
        if (IsAlive()) Move();
    }

    // -- Utility Methods --

    protected override void WasShot() {
        Kill();
        if (!printedBlood) PrintBlood();

        standStill = true;
        moveRight = false;

        CalculateTime();
    }
    
    protected override void PrintBlood() {
        base.PrintBlood();
        PrintOuchBlurb();
    }

    private void PrintOuchBlurb() {
        if (activeBlurb != null) return;
        
        activeBlurb = Instantiate(blurbOuch, blurbParent.transform.position, Quaternion.identity);
        activeBlurb.transform.SetParent(canvas.transform, false);
        
        SetBlurbPosition();

        DeleteBlurb();
    }

    private void SetBlurbPosition() {
        if (activeBlurb == null) return;
        
        Vector2 viewportPoint = camera.WorldToViewportPoint(blurbParent.transform.position);
        activeBlurb.GetComponent<RectTransform>().anchorMin = viewportPoint;  
        activeBlurb.GetComponent<RectTransform>().anchorMax = viewportPoint;
    }

    private void DeleteBlurb() {
        var textDeleteTime = deleteBlurbTime - deleteBlurbTextOffset;

        foreach (Transform child in activeBlurb.transform) {
            // Delete children first
            Destroy(child.gameObject, textDeleteTime);
        }
        Destroy(activeBlurb, deleteBlurbTime);
    }
}