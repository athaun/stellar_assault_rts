using UnityEngine;

public class EnemyShip : Ship {
    private Ship spaceStation;

    void Update() {
        Mover.moveTo(spaceStation.transform.position);
    }
}
