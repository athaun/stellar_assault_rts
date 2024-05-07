using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour {
    private static ButtonManager instance;
    private List<InstantiateShips> instantiateShips = new List<InstantiateShips>();
    public Button[] buttons;
    private Button SelectedButton;

    public static ButtonManager Instance {
        get => instance;
    }

    public void SelectButton(Button button) {

        // Prevents the player from moving selected units to the new unit position
        UnitSelectionManager.Instance.clearSelection();

        if (SelectedButton == button) {
            DeselectButton();
            return;
        }

        if (SelectedButton != null) {
            TextMeshProUGUI selectedButtonText = SelectedButton.GetComponentInChildren<TextMeshProUGUI>();
            selectedButtonText.fontStyle = FontStyles.Normal;

            // Reset the color of the previously selected button
            ColorBlock colorBlock = SelectedButton.colors;
            colorBlock.normalColor = Color.white; // Change this to the default color of your buttons
            SelectedButton.colors = colorBlock;
        }

        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.fontStyle = FontStyles.Bold;

        // Change the color of the selected button
        ColorBlock selectedColorBlock = button.colors;
        selectedColorBlock.normalColor = new Color32(85, 216, 255, 255);
        button.colors = selectedColorBlock;

        SelectedButton = button;

        foreach (InstantiateShips instantiateShip in instantiateShips) {
            instantiateShip.enabled = false;
        }

        button.GetComponentInChildren<InstantiateShips>().enabled = true;
    }

    public static Button selectedButton() {
        return instance.SelectedButton;
    }

    public void DeselectButton() {
        if (SelectedButton != null) {
            TextMeshProUGUI selectedButtonText = SelectedButton.GetComponentInChildren<TextMeshProUGUI>();
            selectedButtonText.fontStyle = FontStyles.Normal;

            // Reset the color of the button
            ColorBlock colorBlock = SelectedButton.colors;
            colorBlock.normalColor = Color.white; // Change this to the default color of your buttons
            SelectedButton.colors = colorBlock;

            foreach (InstantiateShips instantiateShip in instantiateShips) {
                instantiateShip.enabled = false;
            }
            SelectedButton = null;
        }
    }

    void Start() {
        foreach (Button button in buttons) {
            instantiateShips.Add(button.GetComponentInChildren<InstantiateShips>());
            instantiateShips[instantiateShips.Count - 1].enabled = false;
            button.onClick.AddListener(() => {
                SelectButton(button);
            });
        }

        instance = this;
        //InstantiateShips.OnShipInstantiated += DeselectButton;
    }

    // void OnDestroy() {
    //     InstantiateShips.OnShipInstantiated -= DeselectButton;
    // }

    // Asher dont touch this.
    // Asher this is to make it so that when you left click (On the UI) it will deselect the button.
    // ok
    void Update() {
        if (SelectedButton != null && Input.GetMouseButtonDown(1)) {
            //DeselectButton();
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0) {
                foreach (RaycastResult result in results) {
                    // Skip any GameObjects with colliders that we want to ignore
                    Collider collider = result.gameObject.GetComponent<Collider>();
                    if (collider != null && collider.gameObject.layer == LayerMask.NameToLayer("Awareness")) {
                        continue;
                    }

                    if (result.gameObject.GetComponent<Button>() != null) {
                        return;
                    }
                }
                DeselectButton();
            }
        }
    }
}