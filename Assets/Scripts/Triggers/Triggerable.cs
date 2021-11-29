using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : BlurbObject {
    
    // -- Components --
    protected Player player;
    
    protected virtual void Update() {
        SetBlurbPosition();
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        player = other.gameObject.GetComponent<Player>();
        if (printedBlurb) return;
    }

    protected virtual void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
    }

    protected virtual void OnTriggerExit2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (activeBlurb != null) DeleteBlurb();
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
        activeBlurb = Instantiate(blurbToCreate.blurb, blurbParent.transform.position, Quaternion.identity);
        activeBlurb.transform.SetParent(canvas.transform, false);
        activeBlurb.transform.SetAsFirstSibling();

        SetBlurbPosition();
    }
}
