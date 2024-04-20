using UnityEngine;
using UnityEngine.AI;

public class EnemyShip : Ship {
    private Ship spaceStation;
    public Ship SpaceStation { get { return spaceStation; } set { spaceStation = value; } }

    public void Start () {
        faction = 1;
        
        mover = GetComponent<UnitMover>();
        outline = GetComponent<Outline>();

        UnitSelectionManager managerInstance = FindFirstObjectByType<UnitSelectionManager>();
        if (managerInstance != null) {
            managerInstance.registerEnemy(this);
        } else {
            Debug.LogError("UnitSelectionManager not found in the scene! Cannot register enemy ship.");
        }

        health = maxHealth;
    }

    void Update() {
        Mover.moveTo(spaceStation.transform.position);

        Outline.enabled = true;
    }
}