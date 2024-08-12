using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum InventoryType {
    Player_Inventory,
    Player_Hotbar,
    Chest1,
    Enemy
}
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] public InventorySystem inventorySystem;
    [SerializeField] public int size;
    [SerializeField] private GameObject droppedItemPrefab;
    [HideInInspector] public PlayerStateMachine playerStateMachine;
    [SerializeField] public InventoryHolderUI inventoryHolderUI;
    [SerializeField] public InventoryType inventoryType;


    private void Awake() {
        inventorySystem = new InventorySystem(size);
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        if(inventoryType == InventoryType.Player_Inventory){
            inventoryHolderUI.Initalize();
        }
    }
    private void OnEnable() {
        playerStateMachine.OnItemPickUp += HandleItemPickup;
    }

    private void OnDisable() {
        playerStateMachine.OnItemPickUp -= HandleItemPickup;
    }

    public void DropInventoryItems()
    {
        for (int i = 0; i < inventorySystem.inventorySlots.Count; i++)
        {
            if (inventorySystem.inventorySlots[i].inventoryItem != null)
            {
                GameObject droppedItem = Instantiate(droppedItemPrefab, this.transform.position, Quaternion.identity);
                droppedItem.AddComponent(typeof(DroppedItemSlot));
                droppedItem.AddComponent(typeof(Rigidbody));
                Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                droppedItem.GetComponent<DroppedItemSlot>().inventorySlot = new InventorySlot(inventorySystem.inventorySlots[i]);
            }
        }
    }
    public void HandleItemPickup(DroppedItemSlot droppedItemSlot) {
        if(inventoryType == InventoryType.Player_Inventory) {
            inventorySystem.AddToInventory(droppedItemSlot.inventorySlot);
        }
    }
}