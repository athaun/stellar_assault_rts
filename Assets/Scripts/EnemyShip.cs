using UnityEngine;

public class EnemyShip : Ship {

    /*
    NOTE: This class will be deleted after the playtest, it is a bad architecture.
    Enemy logic will be moved into the Ship class.
    */ 

    public Ship spaceStation;
    public Ship SpaceStation { get { return spaceStation; } set { spaceStation = value; } }

    void Update() {
        Outline.enabled = true;

        if (isPassive) return;

        if (Targets.Count > 0) {
            Attack(Targets[0]);   
        } else {
            Attack(spaceStation);
        }
    }
}