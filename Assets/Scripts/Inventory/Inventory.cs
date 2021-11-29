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
    
    // -- Variables --
    public bool inventoryOpen = false;
    private List<InventorySlot> inventoryHotSlots = new List<InventorySlot>();
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    
    // -- Components --
    public Button closeButton;
    public GameObject inventoryParent;

    void Start() {
        foreach (Transform child in inventoryParent.transform) {
            if (child.CompareTag("InventorySlot")) inventorySlots.Add(child.GetComponent<InventorySlot>());
            if (child.CompareTag("InventorySlotHot")) inventoryHotSlots.Add(child.GetComponent<InventorySlot>());
        }
        
        closeButton.onClick.AddListener(Activate);
    }
    
    // -- Utility Methods --
    public void Activate() {
        inventoryOpen = !inventoryOpen;
        gameObject.SetActive(inventoryOpen);
    }

    public void AddItem(WorldItem item) {
        if (HasFreeSlot()) GetOpenSlot().AddItem(item);
    }

    public InventorySlot GetOpenSlot() {
        foreach (InventorySlot slot in inventorySlots) {
            if (!slot.containsItem) return slot;
        }

        return default;
    } 

    private bool HasFreeSlot() {
        foreach (InventorySlot slot in inventorySlots) {
            if (!slot.containsItem) return true;
        }

        return false;
    }
}
