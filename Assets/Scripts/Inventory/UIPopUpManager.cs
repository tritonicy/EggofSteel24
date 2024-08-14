using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopUpManager : MonoBehaviour
{
    [SerializeField] GameObject consumableMenuPrefab;
    internal void HandleSlotClick(InventoryItemUI inventoryItemUI, PointerEventData pointerEventData)
    {
        var consumableitem = Instantiate(consumableMenuPrefab, inventoryItemUI.transform.position, quaternion.identity);
        consumableitem.transform.SetParent(this.transform);
    }
}
