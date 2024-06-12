using System.Collections;
using System.Collections.Generic;
using System.Net;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockSystem : MonoBehaviour
{
    private CharacterInput characterInput;
    public bool isLocked = false;
    [SerializeField] GameObject cameraFocus;
    [SerializeField] CinemachineVirtualCamera vcam;
    private GameObject closestEnemy;

    private void Awake() {
        characterInput = new CharacterInput();

        characterInput.Character.Lock.performed += (context) => HandleLock();
    }


    void Update()
    {
        HandleFollow();
    }

    private void OnEnable() {
        characterInput.Character.Enable();
    }
    
    private void OnDisable() {
        characterInput.Character.Disable();
    }

    private void HandleFollow() {

        if(!isLocked) {
            cameraFocus.transform.position = this.transform.position;
        }

        else{
            if(closestEnemy == null) {
                return;
            }
            cameraFocus.transform.position = FindMediumPoint(this.transform.position, closestEnemy.transform.position);
        }
        vcam.Follow = cameraFocus.transform;

    }

    private GameObject FindClosestEnemy(Vector3 initPos, float radius) {
        float minDistance = 9999f;
        GameObject closestEnemy = null;

        Collider[] colliders = Physics.OverlapSphere(initPos, radius);

        if(colliders.Length > 0 ) {
            foreach (Collider collider in colliders)
            {
                if(Vector3.Distance(initPos, collider.gameObject.transform.position) < minDistance && collider.gameObject.tag == "Enemy") {
                    closestEnemy = collider.gameObject;
                    minDistance = Vector3.Distance(initPos, collider.gameObject.transform.position);
                }              
            }
        }
        return closestEnemy;
    }

    private Vector3 FindMediumPoint(Vector3 pos1, Vector3 pos2) {
        return (pos1+pos2) / 2;
    }

    private void HandleLock() {
        isLocked = !isLocked;
        closestEnemy = FindClosestEnemy(this.transform.position, 15f);
        Debug.Log(closestEnemy.gameObject.name);
    }

}
