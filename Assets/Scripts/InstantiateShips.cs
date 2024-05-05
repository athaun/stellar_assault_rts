using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstantiateShips : MonoBehaviour {
    [SerializeField] private Economy economy;
    public static event Action OnShipInstantiated;
    public GameObject selectedShip;
    private Button shipButton;

    void Start() {
        shipButton = GetComponent<Button>();
    }

    void Update() {
        if(economy.Scrap < selectedShip.GetComponent<Ship>().ScrapCost) {
            shipButton.interactable = false;
        } else {
            shipButton.interactable = true;
        }
        if (selectedShip != null && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits) {
                // Skip any awareness colliders which may be in the way
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Awareness")) {
                    continue;
                }

                if (hit.collider.tag == "BasePlane") {
                    Vector3 position = hit.point;
                    position.y = 0.01f;
                    if(economy.Scrap < selectedShip.GetComponent<Ship>().ScrapCost) {
                        Debug.Log("Not enough scrap to build this ship");
                        break;
                    }

                    Instantiate(selectedShip, position, Quaternion.identity);
                    economy.UseScrap(selectedShip.GetComponent<Ship>().ScrapCost);
                    OnShipInstantiated?.Invoke();
                    break;
                }
            }
        }
    }
}
