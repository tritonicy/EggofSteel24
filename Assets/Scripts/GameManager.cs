using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] private PlayerStateMachine playerStateMachine;
    private bool isInventoryOpen = false;
    [SerializeField] private GameObject inventoryPanel;

    private void Awake() {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        playerStateMachine.OnInventoryPressed += HandleInventoryToggle;
        HandleInventoryToggle();
    }
    

    private void HandleInventoryToggle() {
        if(isInventoryOpen) {
            isInventoryOpen = false;
            inventoryPanel.SetActive(false);
        }else {
            isInventoryOpen = true;
            inventoryPanel.SetActive(true);
        }
    }
}
