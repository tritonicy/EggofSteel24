using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public CharacterInput characterInput;
    public LockSystem lockSystem;
    public Vector3 input;
    public Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float circularMoveSpeed = 0.4f;
    [SerializeField] public float jumpSpeed;
    public bool isGrounded = false;
    public bool isFalling;
    [SerializeField] LayerMask groundLayer;
    public bool isJumpPressed = false;
    public bool isJumping = false;
    public float jumpBufferTime = 0.1f;
    public float jumpBufferCounter = 0f;
    public float coyoteTime = 0.2f;
    public float coyoteCounter = 0f;
    float xRate = 1f;
    float zRate = 1f;


    private PlayerBaseState currentState;
    // public get ve setter
    public PlayerBaseState CurrentState{get{return currentState;} set{currentState = value;}}
    public PlayerStateFactory states;
    [SerializeField] float angle;

    
    private void Awake() {
        rb = GetComponentInChildren<Rigidbody>();
        characterInput = new CharacterInput();
        lockSystem = FindObjectOfType<LockSystem>();
        characterInput.Character.Jump.started += OnJump;
        characterInput.Character.Jump.canceled += OnJump;



        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

    }
    private void OnEnable() {
        characterInput.Character.Enable();
    }

    private void OnDisable() {
        characterInput.Character.Disable();    
    }
    private void OnJump(InputAction.CallbackContext context) {
        isJumpPressed = context.ReadValueAsButton();
    }
    
    void Start()
    {
        
    }

    void Update()
    {  
        GatherInput();
        CheckIsGrounded();
        currentState.UpdateState();
        Look();
        Physics.SyncTransforms();
        if(lockSystem.isLocked) {
            float? currentAngle = CalcFirstAngle(lockSystem.closestEnemy);
            if(currentAngle != null) {
                CircularMove(currentAngle);
            }
        }
    }
    private void FixedUpdate() {

        if(!lockSystem.isLocked) Move();
    }

    private void GatherInput() {
        input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));

    }
    private void Move() {
        rb.MovePosition(transform.position + transform.forward * input.magnitude * moveSpeed * Time.deltaTime);
    }

    private void Look() {
        if(input != Vector3.zero) {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
            var changedDir = matrix.MultiplyPoint3x4(input);

            var rot = Quaternion.LookRotation(changedDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation,rot, 360);
        }
    }
    private void CheckIsGrounded() {
        isGrounded = Physics.BoxCast(transform.position, new Vector3(1f,0.2f,1f), -transform.up , out RaycastHit m_Hit, transform.rotation, 1f, groundLayer) ? true : false;
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

        xRate = Mathf.Abs(Mathf.Cos((float) angle * Mathf.Deg2Rad));
        zRate = Mathf.Abs(Mathf.Sin((float) angle * Mathf.Deg2Rad));

        Debug.Log(xRate);
        Debug.Log(zRate);



        if (angle > 0 && angle < 90)
        {
            angle -= input.x * circularMoveSpeed * (1-xRate);
            angle += input.z * circularMoveSpeed * (1-zRate);
        }
        else if (angle < 180)
        {
            angle -= input.x * circularMoveSpeed * (1-xRate);
            angle -= input.z * circularMoveSpeed * (1-zRate);
        }
        else if (angle < 270)
        {
            angle += input.x * circularMoveSpeed * (1-xRate);
            angle -= input.z * circularMoveSpeed * (1-zRate);
        }
        else
        {
            angle += input.x * circularMoveSpeed * (1-xRate);
            angle += input.z * circularMoveSpeed * (1-zRate);
        }

        float x = Mathf.Cos((float)(angle - 45) * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin((float)(angle - 45) * Mathf.Deg2Rad) * radius;

        if (Time.deltaTime > 0)
        {
            transform.position = new Vector3(enemy.transform.position.x + x, transform.position.y, enemy.transform.position.z + z);
        }

    }
    
    private float? CalcFirstAngle(GameObject closestEnemy) {
        if(closestEnemy == null) {return null;}

        Vector3 diff = this.transform.position - closestEnemy.transform.position;
        float angle = (Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg) + 45f;
        if (angle < 0) angle = 360 + angle;
        return angle;
    }
}
