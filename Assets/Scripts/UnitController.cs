using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour {

    private Camera mainCamera;
    private LayerMask groundLayer;

    public GameObject selectedUnit;

    private bool pressed = false;
    private bool move = false;

    private Vector3 newPosition;
    public GameObject newPositionMarker;

    void Awake() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); // set the mainCamera variable to the camera with the tag "MainCamera"
        groundLayer = LayerMask.GetMask("Terrain"); // Set the ground layer (at y = 0) as the "Terrain" layer

        selectedUnit = GameObject.FindGameObjectWithTag("SelectedUnit");

        newPositionMarker = Instantiate(newPositionMarker);

        if (newPositionMarker == null) {
            newPositionMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newPositionMarker.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void findSelectedUnit() {
        selectedUnit = GameObject.FindGameObjectWithTag("SelectedUnit");
        if (selectedUnit == null) {
            move = false;
        }
    }

    public Vector3 getSelectedUnitPosition() {
        return selectedUnit.transform.position;
    }

    private void getMoveToLocation() {
        if (Input.GetMouseButtonDown(1)) {
            // First press, selects a X and Z position on the plane
            newPosition = GetPointUnderCursor();
            pressed = true;
            move = false;

        } else if (Input.GetMouseButtonUp(1)) {
            // The mouse has been released, a position to move to has been stored in a Vector3
            // Move the ship to the new position in 3D space (no pathfinding, it is a straight path)
            pressed = false;
            move = true;

            Ship ship = selectedUnit.GetComponent<Ship>();

            if (ship == null) {
                Debug.Log("CANT FIND SHIP UNIT CONTROLLER");
                return;
            }

            ship.Mover.Speed = ship.Mover.defaultSpeed;
            if (Vector3.Distance(selectedUnit.transform.position, newPosition) < 5) {
                ship.Mover.Speed = 0.3f;
                ship.Mover.closeToTarget = true;
            }
        }

        if (pressed) {
            newPosition.y = 0.001f;
            newPositionMarker.transform.position = newPosition;
        }
    }

    void Update() {
        findSelectedUnit();
        if (selectedUnit != null) {
            getMoveToLocation();
            if (move) {
                // rotateAndMove();
                selectedUnit.GetComponent<UnitMover>().moveTo(newPosition);
                move = false;
            }
        }
    }

    public Vector3 GetPointUnderCursor() {
        // Create a new plane with normal (0, 1, 0) and passing through the origin
        Plane plane = new Plane(new Vector3(0, 1, 0), 0);

        // Initialize a variable to store the distance from the ray origin to the ray intersection of the plane
        float distance;

        // Create a ray from the camera through the mouse position on the screen
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Initialize a variable to store the world position of the mouse
        Vector3 mouseWorldPosition = new Vector3(0, 0, 0);

        // If the ray intersects the plane
        if (plane.Raycast(ray, out distance)) {
            // Get the point at the distance along the ray
            mouseWorldPosition = ray.GetPoint(distance);
        }

        // Initialize a variable to store the hit position of the raycast
        RaycastHit hitPosition;

        // If a raycast from the mouse world position in the direction of the camera's forward vector hits something
        if (Physics.Raycast(mouseWorldPosition, mainCamera.transform.forward, out hitPosition, 100, groundLayer)) {
            // Return the point of the hit
            return hitPosition.point;
        } else {
            // If the raycast doesn't hit anything, return the mouse world position
            return mouseWorldPosition;
        }
    }
}