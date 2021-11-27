using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingFountain : BlurbObject {
    
    // -- Variables --
    public GameObject blurbParent; 
    
    private Image activeBlurb;

    // -- Components --
    public Image blurb;
    
    private CircleCollider2D circleCollider2D;

    // Start is called before the first frame update
    void Start() {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        
    }
    
    private void PrintOuchBlurb() {
        activeBlurb = Instantiate(blurb, blurbParent.transform.position, Quaternion.identity);
        activeBlurb.transform.SetParent(canvas.transform, false);
        
        SetBlurbPosition();

        DeleteBlurb();
    }

    
}
