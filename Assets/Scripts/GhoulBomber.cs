using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhoulBomber : MonoBehaviour {

    private Ship parentShip;

    private void Start() {
        parentShip = GetComponentInParent<Ship>();
    }

    private void Update() {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation);
        foreach (Collider collider in colliders) {
            if (collider.gameObject.layer != LayerMask.NameToLayer("Awareness")) {
                Ship otherShip = collider.GetComponentInParent<Ship>();
                if (otherShip != null && otherShip.faction != parentShip.faction) {
                    otherShip.TakeDamage(Mathf.Clamp(otherShip.MaxHealth, 0, 4500));
                    parentShip.TakeDamage(parentShip.MaxHealth);
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
