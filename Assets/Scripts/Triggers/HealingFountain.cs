using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingFountain : Triggerable {
    
    // -- Variables --
    private bool hasHealed = false;
    private bool healPlayer = false;
    
    void Update() {
        SetBlurbPosition();
        
        if (healPlayer) {
            if (Input.GetKeyDown(KeyCode.E)) {
                player.Heal();
                player.ActivateHealText();
                
                hasHealed = true;
                healPlayer = false;
                
                DeleteBlurb();
                PrintBlurb(GetBlurb("NoHeal"));
            }
        }
    }
    
    protected override void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (printedBlurb) return;

        PrintBlurb(hasHealed ? GetBlurb("NoHeal") : GetBlurb("Heal"));
    }

    protected override void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (hasHealed) return;
        
        player = other.gameObject.GetComponent<Player>();
        healPlayer = true;
    }

    protected override void OnTriggerExit2D(Collider2D other) {
       if (!other.gameObject.CompareTag("Player")) return;
       if (activeBlurb != null) DeleteBlurb();
   }
}
