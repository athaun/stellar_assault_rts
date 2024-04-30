using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour {

    private static UnitSelectionManager instance;
    public static UnitSelectionManager Instance { get => instance; }

    private List<Ship> units = new List<Ship>();
    private List<Ship> selectedUnits = new List<Ship>();
    private List<Ship> enemyUnits = new List<Ship>();

    public List<Ship> Units { get => units; }
    public List<Ship> SelectedUnits { get => selectedUnits; }

    private List<Ship> destroyQueue = new List<Ship>();

    public Color[] factionColors;

    private LayerMask unitMask;

    public RectTransform selectionBox;
    private Vector2 mouseStartPosition;

    void Start() {
        unitMask = LayerMask.GetMask("Unit");
        instance = this;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            selectUnit();
            mouseStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) {
            releaseSelectionBox();
        }

        // Must be after mouseButtonUp, hence the redundancy
        if (Input.GetMouseButton(0)) {
            updateSelectionBox(Input.mousePosition);
        }
    }    

    void LateUpdate() {
        // Destroy ships queued for destruction to avoid modifying the list while iterating
        foreach (Ship ship in destroyQueue) {
            if (ship.IsEnemy) {
                enemyUnits.Remove(ship);
            } else {
                units.Remove(ship);
                selectedUnits.Remove(ship);
            }
            Destroy(ship.gameObject);
        }
        destroyQueue.Clear();
    }

    /*
        Called by the Ship class on awake
    */
    public void registerShip(Ship ship) {
        units.Add(ship);
        ship.Outline.SetColor(factionColors[ship.faction]);
    }

    public void registerEnemy(Ship ship) {
        enemyUnits.Add(ship);
        ship.Outline.SetColor(factionColors[ship.faction]);
    }

    public void removeShip(Ship ship) {
        destroyQueue.Add(ship);
    }

    private bool inSelection(Vector2 position, Bounds bounds) {
        return position.x > bounds.min.x && position.x < bounds.max.x && position.y > bounds.min.y && position.y < bounds.max.y;
    }

    private void updateSelectionBox(Vector2 mousePosition) {
        if (!selectionBox.gameObject.activeInHierarchy) {
            selectionBox.gameObject.SetActive(true);
        }

        float width = mousePosition.x - mouseStartPosition.x;
        float height = mousePosition.y - mouseStartPosition.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = mouseStartPosition + new Vector2(width / 2, height / 2);

        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        foreach (Ship ship in units) {
            if (ship.faction != 0) continue;

            if (inSelection(Camera.main.WorldToScreenPoint(ship.transform.position), bounds)) {
                ship.Outline.enabled = true;
                ship.tag = "SelectedUnit";
                selectedUnits.Add(ship);
            } else {
                // Only unselect the ship if it wasn't selected this frame
                if (ship.selectedByClick) continue;

                ship.Outline.enabled = false;
                ship.tag = "selectableUnit";
                selectedUnits.Remove(ship);
            }
        }
    }

    private void releaseSelectionBox() {
        selectionBox.gameObject.SetActive(false);
    }

    private void selectUnit() {
        Transform unit = getClicked();

        if (unit != null) {
            Ship ship = unit.parent.gameObject.GetComponent<Ship>();

            if (ship != null) {
                if (ship.faction != 0) return;

                if (!selectedUnits.Contains(ship)) {
                    if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) {
                        clearSelection();
                    }

                    ship.tag = "SelectedUnit";
                    ship.Outline.enabled = true;
                    ship.selectedByClick = true;
                    selectedUnits.Add(ship);
                } else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) {
                    ship.tag = "selectableUnit";
                    ship.Outline.enabled = false;
                    ship.selectedByClick = false;
                    selectedUnits.Remove(ship);
                }
            }
        } else if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) {
            clearSelection();
        }
    }

    private void clearSelection() {
        foreach (Ship ship in selectedUnits) {
            ship.tag = "selectableUnit";
            ship.Outline.enabled = false;
            ship.selectedByClick = false;
        }
        selectedUnits.Clear();
    }

    public Transform getClicked() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int awarenessLayer = LayerMask.NameToLayer("Awareness");
        int layerMask = ~(1 << awarenessLayer);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            return hit.transform;
        } else {
            return null;
        }
    }
}