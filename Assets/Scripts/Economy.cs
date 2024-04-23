using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Economy : MonoBehaviour
{
    [SerializeField] private int scrap = 0;
    [SerializeField] private int electricity = 0;

    public int Scrap { get => scrap; set => scrap = value; }
    public int Electricity { get => electricity; set => electricity = value; }

    // Start is called before the first frame update
    //scrap Functions
    public void AddScrap(int amount)
    {
        scrap += amount;
    }
    IEnumerator GenerateScrap(int amount, bool isActive)
    {
        while(isActive)
        {
            yield return new WaitForSeconds(1);
            scrap += amount;
        }
    }
    public void UseScrap(int amount)
    {
        scrap -= amount;
    }
    //End of scrap Functions

    //electricity Functions
    public void AddElectricity(int amount)
    {
        electricity += amount;
    }
    IEnumerator ConsumeElectricity(int amount, bool isActive)
    {
        while(isActive)
        {
            yield return new WaitForSeconds(1);
            electricity -= amount;
        }
    }
    IEnumerator GenerateElectricity(int amount, bool isActive)
    {
        while(isActive)
        {
            yield return new WaitForSeconds(1);
            electricity += amount;
        }
    }
    public void RemoveElectricity(int amount)
    {
        electricity -= amount;
    }
    //End of electricity Functions

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
