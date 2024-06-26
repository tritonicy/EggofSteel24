using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public CharacterInput characterInput;
    public Vector3 input;
    public Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public float jumpingSpeed = 2f;
    public bool isJumpPressed;
    public bool isGrounded = false;
    [SerializeField] LayerMask groundLayer;

    public float gravtity = -9.8f;
    float initialJumpVelocity;
    float maxJumpTime;
    float maxJumpHeight;
    

    private PlayerBaseState currentState;
    // public get ve setter
    public PlayerBaseState CurrentState{get{return currentState;} set{currentState = value;}}
    public PlayerStateFactory states;

    
    private void Awake() {
        rb = GetComponentInChildren<Rigidbody>();
        characterInput = new CharacterInput();
        characterInput.Character.Jump.started += OnJump;
        characterInput.Character.Jump.canceled += OnJump;

        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        SetupJumpVariables();  
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
        isGrounded = Physics.BoxCast(transform.position, transform.localScale*0.5f, -transform.up , out RaycastHit m_Hit, transform.rotation, 1f) ? true : false;
    }

    private void SetupJumpVariables() {
        float timeToApex = maxJumpTime / 2;
        gravtity = (-2 * maxJumpHeight) / MathF.Pow(timeToApex,2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
}
