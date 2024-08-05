using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Classes")]
    [HideInInspector] public CharacterInput characterInput;
    [HideInInspector] private PlayerBaseState currentState;
    [HideInInspector] public LockSystem lockSystem;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public PlayerStateFactory states;

    [Header("Walk Prop")]
    [SerializeField] private float moveSpeed = 5f;
    [HideInInspector] public bool isWalkingPressed = false;
    [Header("Cicrcular Movement")]
    [SerializeField] private float circularMoveSpeed = 0.4f;
    [SerializeField] private float xRate = 1f;
    [SerializeField] private float zRate = 1f;
    [Header("Jump Prop")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] public float jumpSpeed;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isFalling;
    [HideInInspector] public bool isJumpPressed = false;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isDashing = false;
    [SerializeField] public float jumpBufferTime = 0.1f;
    [HideInInspector] public float jumpBufferCounter = 0f;
    [SerializeField ]public float coyoteTime = 0.2f;
    [HideInInspector] public float coyoteCounter = 0f;

    [Header("Dash Prop")]
    [HideInInspector] public bool isDashPressed = false;
    [SerializeField] public float dashForce = 5f;
    [SerializeField] public float dashDuration = 0.75f;
    [HideInInspector] public Vector3 delayedForceToApply;
    [SerializeField] public float DashTimer = 1f;
    [HideInInspector] public float DashTimeCounter;
    [Header("Other")]
    [HideInInspector] public Vector3 input;
    [HideInInspector] public Action<DroppedItemSlot> OnItemPickUp;



    // public get ve setter
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }


    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        characterInput = new CharacterInput();
        lockSystem = FindObjectOfType<LockSystem>();
        characterInput.Character.Jump.started += OnJump;
        characterInput.Character.Jump.canceled += OnJump;
        characterInput.Character.Dash.started += OnDash;
        characterInput.Character.Dash.canceled += OnDash;


        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

    }
    private void OnEnable()
    {
        characterInput.Character.Enable();
    }

    private void OnDisable()
    {
        characterInput.Character.Disable();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        isDashPressed = context.ReadValueAsButton();
    }

    void Start()
    {

    }

    void Update()
    {
        GatherInput();
        CheckIsGrounded();
        currentState.UpdateStates();
        Look();
        Physics.SyncTransforms();
        if (lockSystem.isLocked)
        {
            float? currentAngle = CalcFirstAngle(lockSystem.closestEnemy);
            if (currentAngle != null)
            {
                CircularMove(currentAngle);
            }
        }

        DashTimeCounter += Time.deltaTime;
    }
    private void FixedUpdate()
    {

        if (!lockSystem.isLocked) Move();
    }

    private void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        isWalkingPressed = input.magnitude > 0;
    }
    private void Move()
    {
        rb.MovePosition(transform.position + transform.forward * input.magnitude * moveSpeed * Time.deltaTime);
    }

    private void Look()
    {
        if (input != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var changedDir = matrix.MultiplyPoint3x4(input);

            var rot = Quaternion.LookRotation(changedDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 360);
        }
    }
    private void CheckIsGrounded()
    {
        isGrounded = Physics.BoxCast(transform.position, new Vector3(1f, 0.2f, 1f), -transform.up, out RaycastHit m_Hit, transform.rotation, 1f, groundLayer) ? true : false;
    }

    private void CircularMove(float? angle)
    {
        // TODO = rateler 0 90 180 270 derecelerinde 1 e cok yaklastigi icin karakterler hizlaniyor bir cap atilabilir. 
        GameObject enemy = lockSystem.closestEnemy;
        if (enemy == null)
        {
            return;
        }
        Vector3 diff = this.transform.position - enemy.transform.position;
        float radius = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);

        xRate = Mathf.Clamp(Mathf.Abs(Mathf.Cos((float)angle * Mathf.Deg2Rad)),0f,1f);
        zRate = Mathf.Clamp(Mathf.Abs(Mathf.Sin((float)angle * Mathf.Deg2Rad)),0f,1f);

        Debug.Log($"xRate = {xRate}");
        Debug.Log($"zRate = {zRate}");


        if (angle > 0 && angle < 90)
        {
            angle -= input.x * circularMoveSpeed * (1 - xRate) / radius;
            angle += input.z * circularMoveSpeed * (1 - zRate) / radius;
        }
        else if (angle < 180)
        {
            angle -= input.x * circularMoveSpeed * (1 - xRate) / radius;
            angle -= input.z * circularMoveSpeed * (1 - zRate) / radius;
        }
        else if (angle < 270)
        {
            angle += input.x * circularMoveSpeed * (1 - xRate) / radius;
            angle -= input.z * circularMoveSpeed * (1 - zRate) / radius;
        }
        else
        {
            angle += input.x * circularMoveSpeed * (1 - xRate) / radius;
            angle += input.z * circularMoveSpeed * (1 - zRate) / radius;
        }

        float x = Mathf.Cos((float)(angle - 45) * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin((float)(angle - 45) * Mathf.Deg2Rad) * radius;

        if (Time.deltaTime > 0)
        {
            transform.position = new Vector3(enemy.transform.position.x + x, transform.position.y, enemy.transform.position.z + z);
        }

    }

    private float? CalcFirstAngle(GameObject closestEnemy)
    {
        if (closestEnemy == null) { return null; }

        Vector3 diff = this.transform.position - closestEnemy.transform.position;
        float angle = (Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg) + 45f;
        if (angle < 0) angle = 360 + angle;
        return angle;
    }

    public void ResetDash()
    {
        isDashing = false;
    }
    public void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.TryGetComponent<DroppedItemSlot>(out DroppedItemSlot droppedItemSlot)) {
            OnItemPickUp?.Invoke(droppedItemSlot);
        }
    }
}
