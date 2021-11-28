using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    
    // -- Constants --
    private const int GUN_ONE_HOT_SLOT = 0;
    private const int GUN_TWO_HOT_SLOT = 1;
    private const int POWER_UP_HOT_SLOT = 2;
    private const int COIN_HOT_SLOT = 3;
    
    // -- Variables --.
    private List<InventorySlot> inventoryHotSlots;
    private List<InventorySlot> inventorySlots;
    
    // -- Components --
    public Button closeButton;
    
    private GameObject inventoryParent;

    void Start() {
        inventoryParent = GameObject.FindGameObjectWithTag("InventoryItemParent");
        
        foreach (Transform child in inventoryParent.transform) {
            if (child.CompareTag("InventorySlot")) inventorySlots.Add(child.GetComponent<InventorySlot>());
            if (child.CompareTag("InventorySlotHot")) inventoryHotSlots.Add(child.GetComponent<InventorySlot>());
        }
        
        closeButton.onClick.AddListener(CloseInventory);
    }
    
    // -- Utility Methods --
    private void CloseInventory() {
        gameObject.SetActive(false);
    }
}
