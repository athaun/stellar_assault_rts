using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstantiateShips : MonoBehaviour {
    [SerializeField] private Economy economy;
    //public static event Action OnShipInstantiated;
    public GameObject selectedShip;

    void Start() {
    }

    void Update() {
        if(selectedShip != null && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {
            ButtonManager.Instance.DeselectButton();
        }
        if (selectedShip != null && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits) {
                // Skip any awareness colliders which may be in the way
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Awareness")) {
                    continue;
                }

                if (hit.collider.CompareTag("Unit") || hit.collider.CompareTag("SelectedUnit")) {
                    return;
                }

                if (hit.collider.tag == "BasePlane") {
                    Vector3 position = hit.point;
                    position.y = 0.01f;
                    if (economy.Scrap < selectedShip.GetComponent<Ship>().ScrapCost) {
                        // Debug.Log("Not enough scrap to build this ship");
                        // ButtonManager.Instance.DeselectButton();
                        break;
                    }

                    Instantiate(selectedShip, position, Quaternion.identity);
                    economy.UseScrap(selectedShip.GetComponent<Ship>().ScrapCost);
                    //OnShipInstantiated?.Invoke();
                    break;
                }
            }
        }
    }
}
