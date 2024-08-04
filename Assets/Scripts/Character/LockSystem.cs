using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class LockSystem : MonoBehaviour
{
    private CharacterInput characterInput;
    [SerializeField] CinemachineVirtualCamera vcam;
    public bool isLocked = false;
    private bool canSwap = false;
    [SerializeField] GameObject cameraFocus;
    private Vector2 swapEnemyVector;
    public GameObject closestEnemy;
    [SerializeField] float fov = 30f;
    [SerializeField] float radius = 10f;
    [SerializeField] LayerMask enemyMask;
    private float swapTimer;
    private float swapTimeCounter = 0.4f;
    private void Awake()
    {
        characterInput = new CharacterInput();

        characterInput.Character.Lock.performed += (context) => HandleLock();
        characterInput.Character.LockSwap.performed += (context) => swapEnemyVector = context.ReadValue<Vector2>();
        characterInput.Character.LockSwap.canceled += (context) => swapEnemyVector = Vector3.zero;
    }


    void Update()
    {
        // if(closestEnemy != null) {
        //     Vector3 diff = this.transform.position - closestEnemy.transform.position;
        //     float angle = (Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg) + 45f;
        //     if(angle < 0) angle = 360 + angle;
        //     Debug.Log(angle);
        // }

        if (ControllerDetection.Instance.isControllerActive)
        {

            if (swapEnemyVector.magnitude > 0)
            {
                swapTimer += Time.deltaTime;
            }
            else
            {
                swapTimer = 0f;
                canSwap = false;
            }

            if (swapTimer > swapTimeCounter)
            {
                swapTimer = 0f;
                canSwap = true;
            }

            HandleLockSwap();
        }
        else
        {
            if (Mouse3D.instance.IsMouseMoving())
            {
                swapTimer += Time.deltaTime;
            }
            else
            {
                swapTimer -= Time.deltaTime / 5;
                canSwap = false;
            }

            if (swapTimer > swapTimeCounter)
            {
                swapTimer = 0f;
                canSwap = true;
                Mouse3D.instance.ResetMousePosition();
            }
            else if (swapTimer <= 0) swapTimer = 0f;

            HandleMouseLockSwap();
        }

        HandleFollow();
    }

    private void OnEnable()
    {
        characterInput.Character.Enable();
    }

    private void OnDisable()
    {
        characterInput.Character.Disable();
    }

    private void HandleFollow()
    {

        if (!isLocked)
        {
            cameraFocus.transform.position = this.transform.position;
        }

        else
        {
            if (closestEnemy == null)
            {
                return;
            }
            cameraFocus.transform.position = FindMediumPoint(this.transform.position, closestEnemy.transform.position);
        }
        vcam.Follow = cameraFocus.transform;

    }

    private GameObject FindClosestEnemy(Vector3 initPos, float radius)
    {
        float minDistance = 9999f;
        GameObject closestEnemy = null;

        Collider[] colliders = Physics.OverlapSphere(initPos, radius);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (Vector3.Distance(initPos, collider.gameObject.transform.position) < minDistance && collider.gameObject.tag == "Enemy")
                {
                    closestEnemy = collider.gameObject;
                    minDistance = Vector3.Distance(initPos, collider.gameObject.transform.position);
                }
            }
        }
        return closestEnemy;
    }

    private GameObject FindClosestEnemy(List<GameObject> gameObjects)
    {
        float minDistance = float.MaxValue;

        if (gameObjects.Count > 0)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (Vector3.Distance(cameraFocus.transform.position, gameObject.transform.position) < minDistance)
                {
                    closestEnemy = gameObject;
                    minDistance = Vector3.Distance(cameraFocus.transform.position, gameObject.transform.position);
                }
            }
        }
        return closestEnemy;
    }

    private Vector3 FindMediumPoint(Vector3 pos1, Vector3 pos2)
    {
        return (pos1 + pos2) / 2;
    }

    private void HandleLock()
    {
        closestEnemy = FindClosestEnemy(this.transform.position, 15f);
        isLocked = closestEnemy == null ? isLocked : !isLocked;
    }

    private void HandleLockSwap()
    {
        if (!canSwap)
        {
            return;
        }
        else
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            var changedDir = matrix.MultiplyPoint3x4(new Vector3(swapEnemyVector.x, 0, swapEnemyVector.y));

            Vector3 SwapToLocation = this.transform.position + changedDir * 5f;

            closestEnemy = FindClosestEnemy(SwapToLocation, 5f);
            canSwap = false;
        }
    }

    private void HandleMouseLockSwap()
    {
        if (!canSwap)
        {
            return;
        }
        if (!isLocked)
        {
            return;
        }
        else
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x / 1920, Input.mousePosition.y / 1080);
            Vector2 centerScreen = new Vector2(0.5f, 0.5f);

            Vector2 MouseVector = mousePos - centerScreen;

            float mouseAngletoCenter = Vector3.Angle(Vector2.right, MouseVector);

            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, enemyMask);

            if (rangeChecks.Length != 0)
            {
                List<GameObject> enemies = new List<GameObject>();
                for (int i = 0; i < rangeChecks.Length; i++)
                {
                    Vector2 enemyVectorRelatedtoCenter = (Vector2)Camera.main.WorldToScreenPoint(rangeChecks[i].transform.position) - new Vector2(Screen.width / 2, Screen.height / 2);

                    if (mouseAngletoCenter - fov < Vector3.Angle(Vector2.right, enemyVectorRelatedtoCenter) && Vector3.Angle(Vector2.right, enemyVectorRelatedtoCenter) < mouseAngletoCenter + fov)
                    {
                        enemies.Add(rangeChecks[i].gameObject);
                    }
                }
                closestEnemy = FindClosestEnemy(enemies);
                canSwap = false;
            }
        }
    }
}
