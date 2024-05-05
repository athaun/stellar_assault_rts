using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour {
    [SerializeField] private GameObject ship;
    private string originalText;
    TextMeshProUGUI text;
    
    void Start() {
        text = GetComponent<TextMeshProUGUI>();
        originalText = text.text;
        Ship s = ship.GetComponent<Ship>();
        text.text = originalText + $"\n${s.ScrapCost} Scrap\n{s.MaxHealth} Health\n{s.AttackDamage} Damage";
    }
}
