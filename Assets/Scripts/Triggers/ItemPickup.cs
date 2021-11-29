using UnityEngine;

public class ItemPickup : Triggerable {
    
    // -- Constants --
    private const int ITEM_LAYER = 16;
    
    // -- Variables --
    private bool hasHealed = false;
    private bool healPlayer = false;
    
    // -- Components --
    public Inventory inventory;

    private GameObject item;

    void Start() {
        foreach (Transform child in transform) {
            if (child.gameObject.layer == ITEM_LAYER) item = child.gameObject;
        }
    }
    
    void Update() {
        SetBlurbPosition();
        
        if (Input.GetKeyDown(KeyCode.E)) {
            inventory.AddItem(gameObject);
        }
    }
    
    protected override void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (printedBlurb) return;

        PrintBlurb(hasHealed ? GetBlurb("NoHeal") : GetBlurb("Heal"));
    }

    protected override void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (hasHealed) return;
        
        player = other.gameObject.GetComponent<Player>();
        healPlayer = true;
    }

    protected override void OnTriggerExit2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (activeBlurb != null) DeleteBlurb();
    }
}
