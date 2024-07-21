using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDetection : MonoBehaviour
{
    public static ControllerDetection Instance;
    public bool isControllerActive = false;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }
}
