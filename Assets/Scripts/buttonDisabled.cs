using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class buttonDisabled : MonoBehaviour {

    [SerializeField] private GameObject selectedShip;
    private Button shipButton;

    private bool selected = false;

    void Start() {
        shipButton = GetComponent<Button>();
    }

    void Update() {
        bool canAffordShip = Economy.Instance.Scrap >= selectedShip.GetComponent<Ship>().ScrapCost;
        selected = ButtonManager.selectedButton() == shipButton;        

        if (canAffordShip) {
            ColorBlock cb = shipButton.colors;
            cb.selectedColor = new Color32(85, 216, 255, 255);
            cb.normalColor = selected ? new Color32(85, 216, 255, 255) : Color.white;
            cb.highlightedColor = cb.selectedColor * 0.8f;
            shipButton.colors = cb;
        } else {
            ColorBlock cb = shipButton.colors;
            cb.normalColor = cb.disabledColor;
            cb.highlightedColor = cb.disabledColor * 0.8f;
            shipButton.colors = cb;
        }
    }
}
