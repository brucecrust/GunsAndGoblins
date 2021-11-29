using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
   
    // -- Variables --
    public bool containsItem;
    public bool isHotSlot;
    
    // -- Components --
    private Button selectionButton;
    private Button deselectionButton;
    private Image icon;
    private InventoryItem item;

    private void Start() {
        foreach (Transform child in transform) {
            if (child.CompareTag("InventoryButton")) {
                selectionButton = child.GetComponent<Button>();
                icon = child.GetChild(0).GetComponent<Image>();
            }

            if (child.CompareTag("InventoryRemoveButton")) deselectionButton = child.GetComponent<Button>();
        }
    }
    
    // -- Utility Methods --
    public void AddItem(WorldItem worldItem) {
        item = new InventoryItem(worldItem); 
    }
}
