using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class buttonDisabled : MonoBehaviour
{
    [SerializeField] private Economy economy;
    [SerializeField] private GameObject selectedShip;
    [SerializeField] private Button shipButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        bool canAffordShip = economy.Scrap >= selectedShip.GetComponent<Ship>().ScrapCost;
        shipButton.interactable = canAffordShip;
    }
}
