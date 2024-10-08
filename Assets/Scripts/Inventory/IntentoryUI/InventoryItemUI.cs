using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class InventoryItemUI : MonoBehaviour , IPointerDownHandler
{
    [SerializeField] public Image itemSprite;
    [SerializeField] public TextMeshProUGUI amount;
    [SerializeField] public InventorySlot inventorySlot;
    public Action<InventoryItemUI,PointerEventData> OnSlotClick; 

    public InventoryItemUI(InventorySlot inventorySlot) {
        this.itemSprite.sprite = inventorySlot.inventoryItem.sprite;
        this.amount.text  = inventorySlot.currentStack.ToString();
    }

    public void Init() {
        ClearUISlot();
    }

    public void AssignInventoryItemSlot(InventorySlot inventorySlot) {
        this.inventorySlot = inventorySlot;
        UpdateUISlot();
    }

    public void ClearUISlot() {
        this.itemSprite.sprite = null;
        // this.itemSprite.color = Color.clear;
        this.amount.text = "";
    }

    public void UpdateUISlot() {
        this.itemSprite.sprite = this.inventorySlot.inventoryItem.sprite;
        this.amount.text  = this.inventorySlot.currentStack.ToString();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnSlotClick?.Invoke(this,eventData);
    }
}
