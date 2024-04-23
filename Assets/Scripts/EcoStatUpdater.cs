using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EcoStatUpdater : MonoBehaviour
{
    [SerializeField] private Economy economy;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Scrap: {economy.Scrap}\nElectricity: {economy.NetElectricity}/{economy.Electricity}kW";
    }
}
