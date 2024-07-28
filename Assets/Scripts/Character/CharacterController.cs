using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{ 
    private CharacterInput characterInput;
    private Vector3 input;
    private Rigidbody rb;
    [SerializeField] private float moveSpeed = 5f;


    private void Awake() {
        rb = GetComponentInChildren<Rigidbody>();  
        characterInput = new CharacterInput();
        
    }
    private void OnEnable() {
        characterInput.Character.Enable();
    }

    private void OnDisable() {
        characterInput.Character.Disable();    
    }

    private void Update() {
        GatherInput();
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
}
