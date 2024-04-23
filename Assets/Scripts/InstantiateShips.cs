using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstantiateShips : MonoBehaviour {
    [SerializeField] private Economy economy;
    public static event Action OnShipInstantiated;
    public GameObject selectedShip;

    public ButtonManager buttonManager;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (selectedShip != null && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "BasePlane") {
                    Vector3 position = hit.point;
                    position.y = 0.01f;
                    if(economy.Scrap < selectedShip.GetComponent<Ship>().ScrapCost) {
                        Debug.Log("Not enough scrap to build this ship");
                        return;
                    }
                    Instantiate(selectedShip, position, Quaternion.identity);
                    economy.UseScrap(selectedShip.GetComponent<Ship>().ScrapCost);
                    OnShipInstantiated?.Invoke();
                }
            }
        }
    }
}
