using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private InventoryHolder inventoryHolder;

    private void Awake() {
        inventoryHolder = GetComponent<InventoryHolder>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            inventoryHolder.DropInventoryItems();
            Destroy(this.gameObject);
        }
    }
}
