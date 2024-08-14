using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    SmallHealtPotion,
    MediumHealthPotion
}
[CreateAssetMenu(fileName = "ConsumableInventoryItem")]
public class ConsumableItemSO : InventoryItemSO
{
    public override ItemType ItemType
    {
        get
        {
            return ItemType.Consumable;
        }
    }
    public ConsumableType consumableType;
    private int consumeRate
    {
        get
        {
            switch (consumableType)
            {
                case ConsumableType.SmallHealtPotion:
                    return 10;
                    break;

                case ConsumableType.MediumHealthPotion:
                    return 30;
                    break;

                default:
                    return -1;
            }
        }
    }
}
