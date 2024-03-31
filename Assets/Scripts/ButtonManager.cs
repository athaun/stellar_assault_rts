using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button[] buttons;
    private Button SelectedButton;
    // Start is called before the first frame update
    void SelectButton(Button button)
    {
        if(SelectedButton != null)
        {
            Text selectedButtonText = SelectedButton.GetComponentInChildren<Text>();
            selectedButtonText.fontStyle = FontStyle.Normal;
        }
        Text buttonText = button.GetComponentInChildren<Text>();
        buttonText.fontStyle = FontStyle.Bold;

        SelectedButton = button;
    }
    void Start()
    {
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(() => SelectButton(button));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
