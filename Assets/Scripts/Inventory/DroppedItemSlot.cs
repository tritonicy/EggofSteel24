using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemSlot : MonoBehaviour
{
    public InventorySlot inventorySlot;

    private void Update() {
        if(inventorySlot.currentStack <= 0) {
            Destroy(this.gameObject);
        }
    }
    public DroppedItemSlot() {
        inventorySlot = null;
    }
}
