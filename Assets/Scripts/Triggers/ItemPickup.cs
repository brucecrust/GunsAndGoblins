using UnityEngine;

public class ItemPickup : Triggerable {
    
    // -- Constants --
    private const int ITEM_LAYER = 16;

    // -- Components --
    public Inventory inventory;
    public WorldItem item;

    void Start() {
        foreach (Transform child in transform) {
            if (child.CompareTag("WorldItem")) {
                item = child.GetComponent<WorldItem>();
                Debug.Log("World Item found");
            }
            break;
        }
    }
    
    void Update() {
        SetBlurbPosition();
        
        if (Input.GetKeyDown(KeyCode.E)) {
            inventory.AddItem(item); 
        }
    }
    
    protected override void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (printedBlurb) return;

        PrintBlurb(GetBlurb("Item"));
    }

    protected override void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        player = other.gameObject.GetComponent<Player>();
    }

    protected override void OnTriggerExit2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) return;
        if (activeBlurb != null) DeleteBlurb();
    }
}
