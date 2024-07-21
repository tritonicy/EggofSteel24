using UnityEngine;
using UnityEngine.InputSystem;
public class Mouse3D : MonoBehaviour
{
    [SerializeField] RectTransform cursor;
    [SerializeField] RectTransform cursorParent;
    private Vector3 deltaMouseMove;
    private Vector3 lastMousePos;
    private float changeTimer = 1f;
    private float changeCounter;
    Vector2 anchoredPos;
    [SerializeField] Vector2 deneme;
    public static Mouse3D instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }
    private void Start() {
        Cursor.visible = false;
        ResetMousePosition();
    }
    private void Update() {
        changeCounter += Time.deltaTime;

        if(changeCounter >= changeTimer) {
            changeCounter = 0f;
            lastMousePos = Input.mousePosition;
            deltaMouseMove = Vector3.zero;
            ResetMousePosition();
        }

        deltaMouseMove = (Input.mousePosition - lastMousePos) * Time.deltaTime;

        // RectTransformUtility.ScreenPointToLocalPointInRectangle(cursorParent , deltaMouseMove,null,out anchoredPos);
        // cursor.anchoredPosition = anchoredPos;

        lastMousePos = Input.mousePosition;
    }

    public void ResetMousePosition() {
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
        deltaMouseMove = Vector3.zero;
    }

    public bool IsMouseMoving() {
        return deltaMouseMove.magnitude > 0f || deltaMouseMove.magnitude <0f; 
           } 
}
