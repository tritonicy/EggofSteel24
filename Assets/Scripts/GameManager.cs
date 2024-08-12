using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] private PlayerStateMachine playerStateMachine;
    public bool isInventoryOpen = false;
    [SerializeField] public GameObject inventoryPanel;
    public static GameManager Instance;

    private void Awake() {

        if(Instance == null) {
            Instance = this;
        }
        else{
            Destroy(this.gameObject);
        }

        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        playerStateMachine.OnInventoryPressed += HandleInventoryToggle;
        HandleInventoryToggle();
    }
    
    private void HandleInventoryToggle() {
        if(isInventoryOpen) {
            isInventoryOpen = false;
            inventoryPanel.SetActive(false);
            Mouse3D.instance.DisableCursor();
        }else {
            isInventoryOpen = true;
            inventoryPanel.SetActive(true);
            Mouse3D.instance.EnableCursor();
        }
    }
}
