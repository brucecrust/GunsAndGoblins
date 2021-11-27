using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingFountain : BlurbObject {
    
    // -- Variables --
    private bool hasHealed = false;
    
    // -- Components --

    void Update() {
        SetBlurbPosition();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Triggered");
        if (!other.gameObject.CompareTag("Player")) return;
        if (printedBlurb) return;
        
        PrintBlurb(GetBlurb("Heal"));
        printedBlurb = true;
    }

    void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (hasHealed) return;
        var player = other.gameObject.GetComponent<Player>();
        if (Input.GetKeyUp("e")) player.health = player.initialHealth;
    }

   private void OnTriggerExit2D(Collider2D other) {
       if (!other.gameObject.CompareTag("Player")) return;
       printedBlurb = false;
       DeleteBlurb();
   }
   
   // -- Utility Methods -- 
   protected override void DeleteBlurb() {
       foreach (Transform child in activeBlurb.transform) {
           // Delete children first
           Destroy(child.gameObject);
       }

       Destroy(activeBlurb);
   }
   
   protected override void PrintBlurb(Blurb blurbToCreate) {
       if (activeBlurb != null) return;
        
       activeBlurb = Instantiate(blurbToCreate.blurb, blurbParent.transform.position, Quaternion.identity);
       activeBlurb.transform.SetParent(canvas.transform, false);
        
       SetBlurbPosition();
   }
}
