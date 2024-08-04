using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InventoryItemSO inventoryItem;
    public int currentStack;

    public InventorySlot(InventoryItemSO inventoryItem, int currentStack) {
        this.inventoryItem = inventoryItem;
        this.currentStack = currentStack;
    }

    public InventorySlot() {
        ClearSlot();
    }

    public void ClearSlot() {
        this.inventoryItem = null;
        this.currentStack = -1;
    }

    public void AddToStack(int amount) {
        currentStack += amount;
    }

    public bool IsRoomLeftInStack(int amount) {
        return currentStack + amount <= inventoryItem.maxStack;
    }
}
