using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public CharacterInput characterInput;
    public LockSystem lockSystem;
    public Vector3 input;
    public Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;
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

    
    private PlayerBaseState currentState;
    // public get ve setter
    public PlayerBaseState CurrentState{get{return currentState;} set{currentState = value;}}
    public PlayerStateFactory states;

    
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
        
    }
    private void FixedUpdate() {
        Move();
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

    private void CircularMove() {

    }
}
