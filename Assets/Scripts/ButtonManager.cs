using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {
    private List<InstantiateShips> instantiateShips = new List<InstantiateShips>();
    public Button[] buttons;
    private Button SelectedButton;

    // Start is called before the first frame update
    public void SelectButton(Button button) {
        // if (SelectedButton != null)
        //     SelectedButton = button;
        // {
        //     Text selectedButtonText = SelectedButton.GetComponentInChildren<Text>();
        //     selectedButtonText.fontStyle = FontStyle.Normal;
        // }
        // Text buttonText = button.GetComponentInChildren<Text>();
        // buttonText.fontStyle = FontStyle.Bold;

        Debug.Log(button.name);
    }

    void Start() {
        foreach (Button button in buttons) {
            button.onClick.AddListener(() => {
                Debug.Log("BUTTON WAS CLICKED! " + button.name);
            });
            instantiateShips.Add(button.GetComponentInChildren<InstantiateShips>());
            instantiateShips[instantiateShips.Count - 1].enabled = false;
        }

    }

    // Update is called once per frame
    void Update() {

    }
}
