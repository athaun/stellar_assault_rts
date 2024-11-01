using System.Collections;
using UnityEngine;

public class RTSCameraController : MonoBehaviour {

    /* 
    TODO:
    - smooth zooming
    - accessible variables
    */

    [Header("Camera")]
    public Camera cam;
    public GameObject cameraBox;
    public float movementSpeed;

    [Header("Camera Rotation")]
    public float sensitivity = 2;
    public float smoothing = 3;
    private Vector2 targetDirection;
    private Vector2 targetCharacterDirection;
    private Quaternion camRotation;
    public float cameraRotationSpeed = 7F;

    [Header("Camera zoom")]
    public float zoomSpeed;
    public float minZoomDist;
    public float maxZoomDist;

    // private bool moveToUnit = false;
    // private Vector3 unitPos = Vector3.positiveInfinity;

    private void Start() {
        cam.cullingMask = ~(1 << LayerMask.NameToLayer("Minimap"));

    }

    void Update() {
        move();
        zoom();
        rotate();
    }


    void move() {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // TODO: implement for final game, its buggy rn
        // if (xInput != 0 || zInput != 0) {
        //     moveToUnit = false;
        // }

        // if (Input.GetKey(KeyCode.Space)) {
        //     moveToUnit = true;
        // }

        // if (moveToUnit) {
        //     unitPos = GameObject.FindGameObjectWithTag("UnitController").GetComponent<UnitController>().getSelectedUnitPosition();
        //         transform.position = Vector3.Lerp(transform.position, unitPos, Time.deltaTime * movementSpeed);
        //         if (Vector3.Distance(unitPos, transform.position) < 0.01) {
        //             moveToUnit = false;
        //             unitPos = Vector3.positiveInfinity;
        //         }
            
        // }

        Vector3 direction = transform.forward * zInput + transform.right * xInput;

        transform.position = Vector3.Lerp(transform.position, transform.position + direction, Time.deltaTime * movementSpeed);
    }


    void zoom() {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        Vector3 direction = (transform.position - cam.transform.position).normalized;
        float distance = Vector3.Distance(transform.position, cam.transform.position);

        if (distance < minZoomDist && scrollInput > 0.0f || distance > maxZoomDist && scrollInput < 0.0f) {
            return;
        }

        cam.transform.position += direction * scrollInput * zoomSpeed * Time.deltaTime;

        // Clamp the camera position to be within the min and max distance
        distance = Vector3.Distance(transform.position, cam.transform.position);
        if (distance < minZoomDist) {
            cam.transform.position = transform.position - direction * minZoomDist;
        } else if (distance > maxZoomDist) {
            cam.transform.position = transform.position - direction * maxZoomDist;
        }
    }


    private float absoluteMouse;
    private float smoothMouse;
    private float absoluteKeyDelta;
    private float smoothKeyDelta;

    public void rotate() {

        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Mouse Rotation
        if (Input.GetMouseButton(2)) {
            // Get raw mouse input for a cleaner reading on more sensitive mice.
            float mouseDeltaX = Input.GetAxisRaw("Mouse X");

            // Scale input against the sensitivity setting and multiply that against the smoothing value.
            mouseDeltaX = Vector2.Scale(new Vector2(mouseDeltaX, 0), new Vector2(sensitivity * smoothing, 0)).x;

            // Interpolate mouse movement over time to apply smoothing delta.
            smoothMouse = Mathf.Lerp(smoothMouse, mouseDeltaX, 1f / smoothing);

            // Find the absolute mouse movement value from point zero.
            absoluteMouse += smoothMouse;
        } else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E)) {
            float keyDelta = 0;
            if (Input.GetKey(KeyCode.Q)) {
                keyDelta = 0.1f;
            }

            if (Input.GetKey(KeyCode.E)) {
                keyDelta = -0.1f;
            }

            // Scale input against the sensitivity setting and multiply that against the smoothing value.
            keyDelta = Vector2.Scale(new Vector2(keyDelta, 0), new Vector2(sensitivity * smoothing, 0)).x;

            // Interpolate mouse movement over time to apply smoothing delta.
            smoothKeyDelta = Mathf.Lerp(smoothKeyDelta, keyDelta, smoothing);

            // Find the absolute mouse movement value from point zero.
            absoluteKeyDelta += smoothKeyDelta;
        }

        var yRotation = Quaternion.AngleAxis(absoluteMouse + absoluteKeyDelta, Vector3.up);
        cameraBox.transform.localRotation = Quaternion.Slerp(cameraBox.transform.rotation, yRotation * targetCharacterOrientation, Time.deltaTime * cameraRotationSpeed);
    }
}




