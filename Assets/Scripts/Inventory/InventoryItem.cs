using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem {
    
    // -- Components --
    public WorldItem worldItem;
    
    // -- Constructors --
    public InventoryItem(WorldItem item) {
        worldItem = item;
    }
}
