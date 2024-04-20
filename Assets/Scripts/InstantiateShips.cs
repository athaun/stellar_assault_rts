using UnityEngine;
using UnityEngine.EventSystems;

public class InstantiateShips : MonoBehaviour {
    public GameObject selectedShip;

    void Update() {
        if (selectedShip == null || EventSystem.current.IsPointerOverGameObject()) return;
        if (Input.GetMouseButtonDown(0)) {
            selectedShip = null;
        }
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "BasePlane") {
                    Vector3 position = hit.point;
                    position.y = 0.01f;
                    Instantiate(selectedShip, position, Quaternion.identity);
                }
            }
        }
    }
}
