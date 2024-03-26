using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour {
    
    public GameObject selected;
    LayerMask unitMask;

    public RectTransform selectionBox;
    private Vector2 mouseStartPosition;

    // private List<Unit> selectedUnits = new List<Unit>();

    void Start () {
        unitMask = LayerMask.GetMask("Unit");
        selected = GameObject.FindGameObjectWithTag("SelectedUnit");
    }

    void Update() {     
        if (Input.GetMouseButtonDown(0)) { 
            selectUnit();
            mouseStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) {
            releaseSelectionBox();
        }

        if (Input.GetMouseButton(0)) {
            updateSelectionBox(Input.mousePosition);
        }

        removeOutlinesFromUnselected();
    }

    private bool unitInSelectionBox(Vector2 position, Bounds bounds) {
        return position.x > bounds.min.x && position.x < bounds.max.x && position.y > bounds.min.y && position.y < bounds.max.y;
    }

    void updateSelectionBox (Vector2 mousePosition) {
        if (!selectionBox.gameObject.activeInHierarchy) {
            selectionBox.gameObject.SetActive(true);
        }

        float width = mousePosition.x - mouseStartPosition.x;
        float height = mousePosition.y - mouseStartPosition.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = mouseStartPosition + new Vector2(width / 2, height / 2);

        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        GameObject[] units = GameObject.FindGameObjectsWithTag("selectableUnit");
        foreach (GameObject u in units) {
            if (unitInSelectionBox(Camera.main.WorldToScreenPoint(u.transform.position), bounds)) {
                u.gameObject.GetComponent<Outline>().enabled = true;
                u.gameObject.tag = "SelectedUnit";
                Debug.Log("Unit in selection box");
            } else {
                u.gameObject.GetComponent<Outline>().enabled = false;
                //
                u.gameObject.tag = "selectableUnit";
                Debug.Log("Unit not in selection box");
            }
        }
    }

    void releaseSelectionBox () {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
    }

    void selectUnit () {
        Transform underMouse = GetClickedGameObject();
        
        if (underMouse != null) {
            // If the player selected a unit
            if (selected != null) {
                // Disable selection of last selected unit
                selected.gameObject.tag = "selectableUnit";
                selected.gameObject.GetComponent<Outline>().enabled = false;
            }

            // Enable selection of unit that was clicked on
            selected = underMouse.parent.transform.gameObject;
            selected.gameObject.tag = "SelectedUnit";
            selected.gameObject.GetComponent<Outline>().enabled = true;
        } else {
            // The player clicked away from the unit, so unselect it
            if (selected != null) {
                selected.gameObject.tag = "selectableUnit";
                selected.gameObject.GetComponent<Outline>().enabled = false;
            }
        }
    }

    void removeOutlinesFromUnselected () {
        GameObject[] unselected = GameObject.FindGameObjectsWithTag("selectableUnit");
        foreach (GameObject u in unselected) {
            u.gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    Transform GetClickedGameObject() { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit; 

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitMask)) {
            return hit.transform;
        } else {
            return null; 
        }
    }
}