using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour {

    private List<Ship> units = new List<Ship>();
    private List<Ship> selectedUnits = new List<Ship>();
    private List<EnemyShip> enemyUnits = new List<EnemyShip>();

    public List<Ship> Units { get { return units; } }
    public List<Ship> SelectedUnits { get { return selectedUnits; } }

    public Color[] factionColors;

    private LayerMask unitMask;

    public RectTransform selectionBox;
    private Vector2 mouseStartPosition;

    void Start() {
        unitMask = LayerMask.GetMask("Unit");
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

    /*
        Called by the Ship class on awake
    */
    public void registerShip(Ship ship) {
        units.Add(ship);
        ship.Outline.SetColor(factionColors[ship.faction]);
    }

    public void registerEnemy(EnemyShip ship) {
        enemyUnits.Add(ship);
        ship.Outline.SetColor(factionColors[ship.faction]);
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

    Transform getClicked() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitMask)) {
            return hit.transform;
        } else {
            return null;
        }
    }
}