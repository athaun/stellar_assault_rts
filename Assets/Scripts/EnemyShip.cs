using UnityEngine;
using UnityEngine.AI;

public class EnemyShip : Ship {
    public Ship spaceStation;
    public Ship SpaceStation { get { return spaceStation; } set { spaceStation = value; } }

    BL_Turret turret;

    public void Start () {
        faction = 1;

        mover = GetComponent<UnitMover>();
        outline = GetComponentInChildren<Outline>();

        UnitSelectionManager managerInstance = FindFirstObjectByType<UnitSelectionManager>();
        if (managerInstance != null) {
            managerInstance.registerEnemy(this);
        } else {
            Debug.LogError("UnitSelectionManager not found in the scene! Cannot register enemy ship.");
        }

        turret = GetComponent<BL_Turret>();

        health = maxHealth;
    }

    void Update() {
    
        Outline.enabled = true;

        if (turret == null || spaceStation == null) return;

        if (Vector3.Distance(transform.position, spaceStation.transform.position) < 14) {
            turret.Fire();
            Mover.rotateShip(spaceStation.transform.position);
            Mover.Agent.isStopped = true;
        } else {
            Mover.moveTo(spaceStation.transform.position);
            Mover.Agent.isStopped = false;
        }

        turret.Aim(spaceStation.transform.position);
    }
}