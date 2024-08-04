using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySystem
{
    [SerializeField ]public int invenetorySize;
    public List<InventorySlot> inventorySlots;

    public void Initalize() {
        inventorySlots = new List<InventorySlot>();
    }
}

