using UnityEngine;
using UnityEngine.InputSystem;
public class Mouse3D : MonoBehaviour
{
    public static Mouse3D instance;
    private Vector3 deltaMouseMove;
    private Vector3 lastMousePos;
    private Bounds bounds = new Bounds(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(Screen.width, Screen.height));
    private float changeTimer = 2f;
    private float changeCounter;

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
    }
    private void Start()
    {
        Cursor.visible = false;
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

    public void ResetMousePosition()
    {
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
