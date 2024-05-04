using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Economy : MonoBehaviour
{
    private Ship ship;
    private static int netElectricity = 0;
    [SerializeField] private int scrap = 0;
    [SerializeField] private int electricity = 0;

    public int Scrap { get => scrap; set => scrap = value; }
    public int Electricity { get => electricity; set => electricity = value; }

    public int NetElectricity { get => netElectricity; set => netElectricity = value; }

    // Start is called before the first frame update
    //scrap Functions
    public void AddScrap(int amount)
    {
        scrap += amount;
    }
    public IEnumerator GenerateScrap(int amount, bool isActive)
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
    public IEnumerator ConsumeElectricity(int amount, bool isActive)
    {
        netElectricity -= amount;
        
        while(isActive)
        {
            yield return new WaitForSeconds(1);
            electricity -= amount;
        }
    }
    public IEnumerator GenerateElectricity(int amount, bool isActive)
    {
        netElectricity += amount;
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
