using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryHolderUI : MonoBehaviour
{
    [SerializeField] public List<InventoryItemUI> InventoryItemUIs;
    [HideInInspector] public int size;
    public InventorySystem inventorySystem;
    [SerializeField] GameObject inventoryItemUIPrefab;
    [SerializeField] public Dictionary<InventorySlot,InventoryItemUI> inventorySlotToUI;

    // Inventory type ilerde sorun cikarabilir. simdilik iyi ama holderlari abstractla degistirmen gerekebilir ilerde.
    // inventory display metodu burada olabilir chest icin falan da gerekli olacak.
    public void Initalize(InventorySystem inventorySystem) {
        this.inventorySystem = inventorySystem;
        this.size = inventorySystem.invenetorySize;
        inventorySlotToUI = new Dictionary<InventorySlot, InventoryItemUI>();
        InventoryItemUIs = new List<InventoryItemUI>(size);

        for(int i = 0; i < size ; i++) {
            InventoryItemUI itemUI = Instantiate(inventoryItemUIPrefab, this.transform).GetComponent<InventoryItemUI>();
            itemUI.Init();
            InventoryItemUIs.Add(itemUI);
            inventorySlotToUI.Add(inventorySystem.inventorySlots[i], InventoryItemUIs[i]);
        }
        inventorySystem.OnItemSlotChanged += HandleUISlot;
    }

    public void HandleUISlot(InventorySlot inventorySlot) {
        KeyValuePair<InventorySlot,InventoryItemUI> item = inventorySlotToUI.FirstOrDefault(i => i.Key == inventorySlot);

        if(item.Value != null) {
            item.Value.AssignInventoryItemSlot(item.Key);
        }
    }

}
