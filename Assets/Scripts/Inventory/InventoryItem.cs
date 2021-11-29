using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : WorldItem {

    // -- Utility Methods --
    void Convert(WorldItem item) {
        itemName = item.itemName;
        backSprite = item.backSprite;
        
    }
}
