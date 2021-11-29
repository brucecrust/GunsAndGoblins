using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour {
    
    // -- Properties --
    public string itemName;

    // -- Components --
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite sideSprite;
    
    void Start() {}
    
    // -- Utility Methods --
    public InventoryItem ToInventoryItem() {
        var inventoryItem = gameObject.AddComponent<InventoryItem>();
        inventoryItem.itemName = itemName;
        inventoryItem.frontSprite = frontSprite;
        inventoryItem.backSprite = backSprite;
        inventoryItem.sideSprite = sideSprite;
        return inventoryItem;
    }
}