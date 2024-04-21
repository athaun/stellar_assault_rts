using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstantiateShips : MonoBehaviour {
    public static event Action OnShipInstantiated;
    public GameObject selectedShip;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (selectedShip != null && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "BasePlane") {
                    Vector3 position = hit.point;
                    position.y = 0.01f;
                    Instantiate(selectedShip, position, Quaternion.identity);
                    OnShipInstantiated?.Invoke();
                }
            }
        }
    }
}
