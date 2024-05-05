using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject ship;
    private string originalText;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        originalText = text.text;
        text.text = originalText + $"\n${ship.GetComponent<Ship>().ScrapCost} Scrap";
    }

    // Update is called once per frame
    void Update()
    {
    }
}
