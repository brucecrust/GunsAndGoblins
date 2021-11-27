using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Neutral : NPC {
    
    // -- Variables --


    // -- Components --
    public GameObject blurbParent;
    public Image blurbOuch;
    
    private Image activeBlurb;

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
        base.FixedUpdate();
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
}