using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Mouse3D : MonoBehaviour
{
    public static Mouse3D instance;
    private Vector3 deltaMouseMove;
    private Vector3 lastMousePos;
    private Bounds bounds = new Bounds(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(Screen.width, Screen.height));
    private float changeTimer = 2f;
    private float changeCounter;
    // [SerializeField] public InventorySlot assignedInventorySlot;
    // [SerializeField] public TextMeshProUGUI itemAmount;
    // [SerializeField] public Image itemSrpite;
    // [SerializeField] public GraphicRaycaster graphicRaycaster;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // ClearMouseSlot();
    }
    private void Start()
    {
        ResetMousePosition();

    }
    private void Update()
    {

        changeCounter += Time.deltaTime;

        if (changeCounter >= changeTimer)
        {
            changeCounter = 0f;
            lastMousePos = Input.mousePosition;
            deltaMouseMove = Vector3.zero;
            ResetMousePosition();
        }
        if (!IsCursorInScreen())
        {
            ResetMousePosition();
        }

        deltaMouseMove = (Input.mousePosition - lastMousePos) * Time.deltaTime;

        lastMousePos = Input.mousePosition;

    }

    // Drag and drop sistemini eklemeyecegimiz icin mouse uzerinde esya tasimaya gerek yok. 

    // public void ClearMouseSlot() {
    //     this.itemSrpite = null;
    //     this.itemAmount.text = "";
    //     this.itemSrpite.color = Color.clear;
    // }

    // public void AssignInventoryItemSlot(InventorySlot inventorySlot) {
    //     this.assignedInventorySlot = inventorySlot;
    //     UpdateMouseSlot();
    //     this.itemSrpite.color = Color.white;
    // }

    // public void UpdateMouseSlot() {
    //     this.itemAmount.text = assignedInventorySlot.currentStack.ToString();
    //     this.itemSrpite.sprite = assignedInventorySlot.inventoryItem.sprite;
    // }
    public void EnableCursor() {
        ResetMousePosition();
        Cursor.visible = true;
    }
    public void DisableCursor() {
        Cursor.visible = false;
    }

    public void ResetMousePosition()
    {
        if(GameManager.Instance.isInventoryOpen) {
            return;
        }
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
        deltaMouseMove = Vector3.zero;
    }

    public bool IsMouseMoving()
    {
        return deltaMouseMove.magnitude > 0f || deltaMouseMove.magnitude < 0f;
    }

    private bool IsCursorInScreen()
    {
        return bounds.Contains(Input.mousePosition);
    }
}
