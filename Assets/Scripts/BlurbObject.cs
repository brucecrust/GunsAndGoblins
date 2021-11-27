using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurbObject : WorldObject {
    
    // -- Variables --
    public float deleteBlurbTime = 2;
    public float deleteBlurbTextOffset = 0.05f;
    
    // -- Components --
    public Image blurb;
    public GameObject blurbParent; 
    
    private Image activeBlurb;
    
    // -- Utility Methods --
    protected virtual void DeleteBlurb() {
        var textDeleteTime = deleteBlurbTime - deleteBlurbTextOffset;

        foreach (Transform child in activeBlurb.transform) {
            // Delete children first
            Destroy(child.gameObject, textDeleteTime);
        }

        Destroy(activeBlurb, deleteBlurbTime);
    }
    
    protected virtual void SetBlurbPosition() {
        if (activeBlurb == null) return;
        
        Vector2 viewportPoint = camera.WorldToViewportPoint(blurbParent.transform.position);
        activeBlurb.GetComponent<RectTransform>().anchorMin = viewportPoint;  
        activeBlurb.GetComponent<RectTransform>().anchorMax = viewportPoint;
    }
    
}
