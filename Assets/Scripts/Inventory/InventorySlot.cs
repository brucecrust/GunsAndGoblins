using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
   
    // -- Variables --
    public bool isHotSlot;
    
    // -- Components --
    public Button selectionButton;
    public Button deselectionButton;
    public Image icon;

    private void Start() {
        foreach (Transform child in transform) {
            if (child.CompareTag("InventoryButton")) {
                selectionButton = child.GetComponent<Button>();
                icon = child.GetChild(0).GetComponent<Image>();
            }

            if (child.CompareTag("InventoryRemoveButton")) deselectionButton = child.GetComponent<Button>();
        }
    }
}
