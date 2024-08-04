using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem")]
public class InventoryItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int maxStack;
    public Sprite sprite;
}
