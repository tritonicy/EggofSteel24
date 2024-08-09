using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryHolderUI : MonoBehaviour
{
    [SerializeField] public List<InventoryItemUI> InventoryItemUIs;
    [HideInInspector] public int size;
    [SerializeField] InventoryHolder inventoryHolder;
    [SerializeField] GameObject inventoryItemUIPrefab;
    [SerializeField] public Dictionary<InventorySlot,InventoryItemUI> inventorySlotToUI;
    [SerializeField] InventoryType InventoryType;

    //Inventory type ilerde sorun cikarabilir. simdilik iyi ama holderlari abstractla degistirmen gerekebilir ilerde.
    public void PlayerInitalize() {
        this.size = inventoryHolder.size;
        inventorySlotToUI = new Dictionary<InventorySlot, InventoryItemUI>();
        InventoryItemUIs = new List<InventoryItemUI>(size);

        for(int i = 0; i < size ; i++) {
            InventoryItemUI itemUI = Instantiate(inventoryItemUIPrefab, this.transform).GetComponent<InventoryItemUI>();
            itemUI.Init();
            InventoryItemUIs.Add(itemUI);
            inventorySlotToUI.Add(inventoryHolder.inventorySystem.inventorySlots[i], InventoryItemUIs[i]);
        }
        inventoryHolder.inventorySystem.OnItemSlotChanged += HandleUISlot;

    }

    public void HandleUISlot(InventorySlot inventorySlot) {
        KeyValuePair<InventorySlot,InventoryItemUI> item = inventorySlotToUI.FirstOrDefault(i => i.Key == inventorySlot);

        if(item.Value != null) {
            item.Value.AssignInventoryItemSlot(item.Key);
        }
    }

}
