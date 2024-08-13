using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InventorySystem
{
    [HideInInspector] public int invenetorySize;
    [SerializeField] public List<InventorySlot> inventorySlots;
    public Action<InventorySlot> OnItemSlotChanged;
    [SerializeField] public InventoryType InventoryType;

    public InventorySystem(int size, InventoryType inventoryType) {
        this.InventoryType = inventoryType;
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i <size ; i++) {
            inventorySlots.Add(new InventorySlot());
        }
        this.invenetorySize = size;
    }

    public void AddToInventory(InventorySlot otherSlot)
    {
        if (ContainsItem(otherSlot.inventoryItem, out InventorySlot itemSlot))
        {
            int roomLeft = itemSlot.HowMuchRoomLeftInStack();
            if (otherSlot.currentStack <= roomLeft)
            {
                itemSlot.AddToStack(otherSlot.currentStack);
                otherSlot.RemoveFromStack(otherSlot.currentStack);
            }
            else
            {
                itemSlot.AddToStack(roomLeft);
                otherSlot.RemoveFromStack(roomLeft);
            }
            OnItemSlotChanged?.Invoke(otherSlot);
            OnItemSlotChanged?.Invoke(itemSlot);
        }
        else if (HasFreeSlot(out InventorySlot freeSlot))
        {
            int roomLeft = otherSlot.inventoryItem.maxStack;
            if (otherSlot.currentStack <= roomLeft)
            {
                freeSlot.UpdateInventorySlot(otherSlot.inventoryItem, otherSlot.currentStack);
                otherSlot.RemoveFromStack(otherSlot.currentStack);
            }
            else
            {
                freeSlot = new InventorySlot(otherSlot.inventoryItem, roomLeft);
                otherSlot.RemoveFromStack(roomLeft);
            }
            OnItemSlotChanged?.Invoke(otherSlot);
            OnItemSlotChanged?.Invoke(freeSlot);
        }
    }

    public bool ContainsItem(InventoryItemSO inventoryItem, out InventorySlot itemSlot) {
        List<InventorySlot> tempinventorySlots = this.inventorySlots.Where(i => i.inventoryItem == inventoryItem).ToList();

        if(tempinventorySlots.Count == 0) {
            itemSlot = null;
            return false;
        }

        foreach (var item in tempinventorySlots)
        {
            if(item.currentStack != item.inventoryItem.maxStack) {
                itemSlot = item;
                return true;
            }
        }
        itemSlot = null;
        return false;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot) {
        freeSlot = this.inventorySlots.FirstOrDefault(i => i.inventoryItem == null);

        if(freeSlot == null) return false;

        return true; 
    }
}

