using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Blurb {
    public string name;
    public Image blurb;
}

public class BlurbObject : WorldObject {
    
    // -- Variables --
    public float deleteBlurbTime = 2;
    public float deleteBlurbTextOffset = 0.05f;

    protected bool printedBlurb;
    
    // -- Components --
    public Blurb[] blurbs;
    public GameObject blurbParent; 
    
    protected Image activeBlurb;
    
    // -- Utility Methods --
    protected virtual void DeleteBlurb() {
        var textDeleteTime = deleteBlurbTime - deleteBlurbTextOffset;

        foreach (Transform child in activeBlurb.transform) {
            // Delete children first
            Destroy(child.gameObject, textDeleteTime);
        }

        Destroy(activeBlurb, deleteBlurbTime);
    }

    protected virtual Blurb GetBlurb(string name) {
        foreach (Blurb blurb in blurbs) {
            if (blurb.name == name) return blurb;
        }

        return default;
    }

    protected virtual void PrintBlurb(Blurb blurbToCreate) {
        if (activeBlurb != null) return;
        
        activeBlurb = Instantiate(blurbToCreate.blurb, blurbParent.transform.position, Quaternion.identity);
        activeBlurb.transform.SetParent(canvas.transform, false);
        activeBlurb.transform.SetAsFirstSibling();
        
        SetBlurbPosition();

        DeleteBlurb();
    }
    
    protected virtual void SetBlurbPosition() {
        if (activeBlurb == null) return;
        
        Vector2 viewportPoint = camera.WorldToViewportPoint(blurbParent.transform.position);
        activeBlurb.GetComponent<RectTransform>().anchorMin = viewportPoint;  
        activeBlurb.GetComponent<RectTransform>().anchorMax = viewportPoint;
    }
    
}
