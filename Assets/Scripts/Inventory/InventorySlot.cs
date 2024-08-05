using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
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
    public InventorySlot(InventorySlot inventorySlot) {
        this.inventoryItem = inventorySlot.inventoryItem;
        this.currentStack = inventorySlot.currentStack;
    }

    public void ClearSlot() {
        this.inventoryItem = null;
        this.currentStack = -1;
    }
    public void UpdateInventorySlot(InventoryItemSO inventoryItemSO, int amount) {
        this.inventoryItem = inventoryItemSO;
        this.currentStack = amount;
        
    }

    public void AddToStack(int amount) {
        currentStack += amount;
    }

    public void RemoveFromStack(int amount) {
        currentStack -= amount;
    }

    public int HowMuchRoomLeftInStack() {
        return inventoryItem.maxStack - currentStack;
    }
    
}
