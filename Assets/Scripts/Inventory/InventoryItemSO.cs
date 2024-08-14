using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType{
    Weapon,
    Armor,
    Consumable
}
public abstract class InventoryItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int maxStack;
    public Sprite sprite;
    public abstract ItemType ItemType{get;}
}
