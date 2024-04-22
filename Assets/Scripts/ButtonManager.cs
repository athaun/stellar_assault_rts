using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour {
    private List<InstantiateShips> instantiateShips = new List<InstantiateShips>();
    public Button[] buttons;
    private Button SelectedButton;

    public void SelectButton(Button button) {
        if(SelectedButton == button) {
            DeselectButton();
            return;
        }

        if (SelectedButton != null)
        {
            TextMeshProUGUI selectedButtonText = SelectedButton.GetComponentInChildren<TextMeshProUGUI>();
            selectedButtonText.fontStyle = FontStyles.Normal;
        }

        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.fontStyle = FontStyles.Bold;

        SelectedButton = button;

        foreach(InstantiateShips instantiateShip in instantiateShips)
        {
            instantiateShip.enabled = false;
        }

        button.GetComponentInChildren<InstantiateShips>().enabled = true;
    }

    public void DeselectButton() {
        if (SelectedButton != null)
        {
            TextMeshProUGUI selectedButtonText = SelectedButton.GetComponentInChildren<TextMeshProUGUI>();
            selectedButtonText.fontStyle = FontStyles.Normal;
            foreach (InstantiateShips instantiateShip in instantiateShips)
            {
                instantiateShip.enabled = false;
            }
            SelectedButton = null;
        }
        Debug.Log("FN CALLED ANYWAYS");
    }

    void Start() {
        foreach (Button button in buttons) {
            instantiateShips.Add(button.GetComponentInChildren<InstantiateShips>());
            instantiateShips[instantiateShips.Count - 1].enabled = false;
            button.onClick.AddListener(() => {
                SelectButton(button);
            });
            
        }
        InstantiateShips.OnShipInstantiated += DeselectButton;
    }

    void OnDestroy() {
        InstantiateShips.OnShipInstantiated -= DeselectButton;
    }

    void Update() {
        if(SelectedButton != null && Input.GetMouseButtonDown(0)) {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if(results.Count > 0)
            {
                foreach(RaycastResult result in results)
                {
                    if(result.gameObject.GetComponent<Button>() != null)
                    {
                        return;
                    }
                }
            }
            // if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            // {
            //     DeselectButton();
            //     Debug.Log("IF CALLED");
            // }
        }
    }
}